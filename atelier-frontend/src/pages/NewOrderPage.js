import React, { useState, useEffect } from 'react';
import './NewOrderPage.css';
import { useNavigate } from 'react-router-dom';

function NewOrderPage({ services, currentUser }) {
    const navigate = useNavigate();
    const [selectedServices, setSelectedServices] = useState([]);
    const [step, setStep] = useState('services');
    const [customerInfo, setCustomerInfo] = useState({ name: '', phone: '' });
    const [userData, setUserData] = useState(null);
    const [loading, setLoading] = useState(false);
    const [showPersonalInfo, setShowPersonalInfo] = useState(true);

    useEffect(() => {
        checkUserData();
    }, [currentUser]);

    const checkUserData = async () => {
        try {
            const response = await fetch(`http://localhost:5041/api/users/by-username/${currentUser}`);

            if (response.ok) {
                const user = await response.json();
                setUserData(user);

                const hasPersonalInfo = user.FirstName && user.LastName && user.Phone;

                try {
                    const userId = user.Id || user.id;

                    if (userId) {
                        const ordersResponse = await fetch(`http://localhost:5041/api/users/${userId}/orders`);
                        if (ordersResponse.ok) {
                            const orders = await ordersResponse.json();

                            const shouldShowPersonalInfo = !hasPersonalInfo && orders.length === 0;
                            setShowPersonalInfo(shouldShowPersonalInfo);

                            if (hasPersonalInfo) {
                                setCustomerInfo({
                                    name: `${user.FirstName} ${user.LastName}`,
                                    phone: user.Phone
                                });
                            }
                        }
                    }
                } catch (error) {
                    setShowPersonalInfo(!hasPersonalInfo);
                }
            } else {
                setShowPersonalInfo(true);
            }
        } catch (error) {
            setShowPersonalInfo(true);
        }
    };

    const toggleService = (service) => {
        setSelectedServices(prev =>
            prev.find(s => s.id === service.id)
                ? prev.filter(s => s.id !== service.id)
                : [...prev, service]
        );
    };

    const totalPrice = selectedServices.reduce((sum, service) => sum + service.price, 0);

    const isValidPhone = (phone) => phone && phone.replace(/\D/g, '').length === 11;

    const formatPhone = (value) => {
        const digits = value.replace(/\D/g, '').substring(0, 11);
        if (!digits) return '';
        return `+7 (${digits.substring(1, 4)}) ${digits.substring(4, 7)}-${digits.substring(7, 9)}-${digits.substring(9, 11)}`;
    };

    const proceedToOrder = () => {
        if (selectedServices.length === 0) {
            alert('Выберите услуги');
            return;
        }

        if (showPersonalInfo) {
            setStep('info');
        } else {
            createOrder();
        }
    };

    const saveUser = async () => {
        try {
            const [firstName, ...lastNameParts] = customerInfo.name.split(' ');
            const lastName = lastNameParts.join(' ') || '';

            const userDataToSave = {
                tgUsername: currentUser,
                firstName: firstName,
                lastName: lastName,
                phone: customerInfo.phone,
                role: "Customer",
            };

            let existingUser = null;
            try {
                const findResponse = await fetch(`http://localhost:5041/api/users/by-username/${currentUser}`);
                if (findResponse.ok) {
                    existingUser = await findResponse.json();
                }
            } catch (error) {
                return false;
            }

            let response;
            let url;

            if (existingUser) {
                const userId = existingUser.Id || existingUser.id;
                if (!userId) return false;

                url = `http://localhost:5041/api/users/${userId}`;
                userDataToSave.id = userId;
                response = await fetch(url, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(userDataToSave)
                });
            } else {
                url = 'http://localhost:5041/api/users';
                response = await fetch(url, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(userDataToSave)
                });
            }

            if (response.ok) {
                const savedUser = await response.json();
                setUserData(savedUser);
                setShowPersonalInfo(false);
                await checkUserData();
                return true;
            }
            return false;
        } catch (error) {
            return false;
        }
    };

    const createOrder = async () => {
        if (loading) return;
        setLoading(true);

        try {
            if (showPersonalInfo) {
                if (!customerInfo.name || !customerInfo.phone) {
                    alert('Заполните все поля');
                    return;
                }

                if (!isValidPhone(customerInfo.phone)) {
                    alert('Введите корректный номер телефона (11 цифр)');
                    return;
                }

                const saved = await saveUser();
                if (!saved) {
                    alert('Ошибка при сохранении данных пользователя');
                    return;
                }
            }

            const orderData = {
                CustomerName: customerInfo.name,
                TgUsername: currentUser,
                Phone: customerInfo.phone,
                ServiceIds: selectedServices.map(s => s.id),
                ServiceNames: selectedServices.map(s => s.name),
                TotalPrice: totalPrice,
                Status: "New"
            };

            const response = await fetch('http://localhost:5041/api/orders', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(orderData)
            });

            if (!response.ok) {
                throw new Error('Ошибка создания заказа');
            }

            await checkUserData();
            alert('Заказ успешно создан!');
            navigate('/profile');

        } catch (error) {
            alert('Ошибка при создании заказа');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="new-order-page">
            <div className="container">
                <button className="back-btn" onClick={() => navigate('/profile')}>← Назад</button>

                <h2>Новый заказ</h2>

                <div className="steps">
                    <div className={`step ${step === 'services' ? 'active' : ''}`}>1. Услуги</div>
                    {showPersonalInfo && <div className={`step ${step === 'info' ? 'active' : ''}`}>2. Данные</div>}
                </div>

                {step === 'services' && (
                    <div className="services-step">
                        <h3>Выберите услуги</h3>

                        <div className="services-grid">
                            {services.map(service => (
                                <div
                                    key={service.id}
                                    className={`service-card ${selectedServices.find(s => s.id === service.id) ? 'selected' : ''}`}
                                    onClick={() => toggleService(service)}
                                >
                                    <div className="checkbox">
                                        {selectedServices.find(s => s.id === service.id) && '✓'}
                                    </div>
                                    <h4>{service.name}</h4>
                                    <p>{service.description}</p>
                                    <div className="price">{service.price} ₽</div>
                                </div>
                            ))}
                        </div>

                        {selectedServices.length > 0 && (
                            <div className="summary">
                                <h4>Выбранные услуги:</h4>
                                {selectedServices.map(service => (
                                    <div key={service.id} className="item">
                                        <span>{service.name}</span>
                                        <span>{service.price} ₽</span>
                                    </div>
                                ))}
                                <div className="total">Итого: {totalPrice} ₽</div>
                            </div>
                        )}

                        <button
                            className="btn-primary"
                            onClick={proceedToOrder}
                            disabled={selectedServices.length === 0 || loading}
                        >
                            {showPersonalInfo ? 'Продолжить' : (loading ? 'Создание...' : 'Создать заказ')}
                        </button>
                    </div>
                )}

                {step === 'info' && showPersonalInfo && (
                    <div className="info-step">
                        <h3>Ваши данные для заказа</h3>

                        <div className="order-preview">
                            <h4>Ваш заказ:</h4>
                            {selectedServices.map(service => (
                                <div key={service.id} className="item">
                                    <span>{service.name}</span>
                                    <span>{service.price} ₽</span>
                                </div>
                            ))}
                            <div className="total">Итого: {totalPrice} ₽</div>
                        </div>

                        <div className="form">
                            <div className="form-group">
                                <label>ФИО *</label>
                                <input
                                    type="text"
                                    value={customerInfo.name}
                                    onChange={(e) => setCustomerInfo({...customerInfo, name: e.target.value})}
                                    placeholder="Иван Иванов"
                                    disabled={loading}
                                />
                            </div>

                            <div className="form-group">
                                <label>Телефон *</label>
                                <input
                                    type="tel"
                                    value={customerInfo.phone}
                                    onChange={(e) => setCustomerInfo({...customerInfo, phone: formatPhone(e.target.value)})}
                                    placeholder="+7 (999) 999-99-99"
                                    disabled={loading}
                                />
                                {customerInfo.phone && !isValidPhone(customerInfo.phone) && (
                                    <div className="error">Введите 11 цифр номера телефона</div>
                                )}
                            </div>

                            <div className="user-note">
                                <p><strong>Telegram:</strong> @{currentUser}</p>
                                <p>Эти данные сохранятся для будущих заказов</p>
                            </div>
                        </div>

                        <div className="actions">
                            <button
                                className="btn-secondary"
                                onClick={() => setStep('services')}
                                disabled={loading}
                            >
                                ← Назад к услугам
                            </button>
                            <button
                                className="btn-primary"
                                onClick={createOrder}
                                disabled={!customerInfo.name || !customerInfo.phone || !isValidPhone(customerInfo.phone) || loading}
                            >
                                {loading ? 'Создание...' : 'Создать заказ'}
                            </button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}

export default NewOrderPage;