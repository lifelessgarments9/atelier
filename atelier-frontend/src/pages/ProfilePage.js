import React, { useState, useEffect } from 'react';
import './ProfilePage.css';
import { useNavigate } from 'react-router-dom';

function ProfilePage({ user, onLogout }) {
    const navigate = useNavigate();
    const [activeTab, setActiveTab] = useState('profile');
    const [orders, setOrders] = useState([]);
    const [settings, setSettings] = useState({
        notifications: true,
    });

    // Статические данные пользователя
    const userData = {
        username: user,
        telegramVerified: true,
        joinDate: '2025-01-26',
        phone: '+7 (999) 123-45-67',
    };

    const loadUserOrders = async () => {
        try {
            console.log('Загрузка заказов для пользователя:', user);
            const response = await fetch(`http://localhost:5041/api/orders`);
            if (response.ok) {
                const allOrders = await response.json();
                console.log('Все заказы из API:', allOrders);

                // Фильтруем заказы текущего пользователя
                const userOrders = allOrders.filter(order => {
                    const orderUsername = order.tgUsername || order.TgUsername;
                    const cleanOrderUsername = orderUsername?.replace('@', '');
                    const cleanUser = user?.replace('@', '');
                    return cleanOrderUsername === cleanUser;
                });

                console.log('Отфильтрованные заказы пользователя:', userOrders);
                setOrders(userOrders);
            } else {
                console.error('Ошибка HTTP:', response.status);
            }
        } catch (error) {
            console.error('Ошибка загрузки заказов:', error);
        }
    };

    useEffect(() => {
        if (user) {
            loadUserOrders();
        }
    }, [user]);

    const handleCreateOrder = () => {
        navigate('/neworder');
    };

    const handleSettingChange = (setting) => {
        setSettings(prev => ({
            ...prev,
            [setting]: !prev[setting]
        }));
    };

    const getStatusText = (status) => {
        const statusMap = {
            'completed': 'Завершен',
            'in_progress': 'В работе',
            'pending': 'Ожидает',
            'new': 'Новый'
        };
        return statusMap[status] || status;
    };

    const getStatusClass = (status) => {
        const classMap = {
            'completed': 'status-completed',
            'in_progress': 'status-in-progress',
            'pending': 'status-pending',
            'new': 'status-pending'
        };
        return classMap[status] || '';
    };

    // Функция для отладки - посмотреть структуру заказа
    const debugOrder = (order) => {
        console.log('Структура заказа:', order);
    };

    return (
        <div className="profile-page">
            <div className="profile-container">
                <button className="back-btn" onClick={() => navigate('/')}>
                    ← Назад на главную
                </button>

                <div className="profile-header">
                    <h2>Личный кабинет</h2>
                </div>

                <div className="profile-tabs">
                    <button
                        className={`profile-tab ${activeTab === 'profile' ? 'active' : ''}`}
                        onClick={() => setActiveTab('profile')}
                    >
                        👤 Профиль
                    </button>
                    <button
                        className={`profile-tab ${activeTab === 'orders' ? 'active' : ''}`}
                        onClick={() => setActiveTab('orders')}
                    >
                        📦 Мои заказы
                    </button>
                    <button
                        className={`profile-tab ${activeTab === 'settings' ? 'active' : ''}`}
                        onClick={() => setActiveTab('settings')}
                    >
                        ⚙️ Настройки
                    </button>
                </div>

                {/* --- ПРОФИЛЬ --- */}
                {activeTab === 'profile' && (
                    <div className="profile-content">
                        <div className="profile-info-card">
                            <div className="profile-avatar-section">
                                <div className="profile-avatar">
                                    👤
                                </div>
                                <div className="avatar-actions">
                                    <button className="avatar-btn">Изменить фото</button>
                                </div>
                            </div>

                            <div className="profile-details">
                                <div className="detail-row">
                                    <span className="detail-label">Имя пользователя:</span>
                                    <span className="detail-value">@{userData.username}</span>
                                </div>
                                <div className="detail-row">
                                    <span className="detail-label">Telegram:</span>
                                    <span className="detail-value verified">
                                        Подтвержден ✅
                                    </span>
                                </div>
                                <div className="detail-row">
                                    <span className="detail-label">Дата регистрации:</span>
                                    <span className="detail-value">{userData.joinDate}</span>
                                </div>
                                <div className="detail-row">
                                    <span className="detail-label">Телефон:</span>
                                    <span className="detail-value">{userData.phone}</span>
                                </div>
                            </div>

                            <div className="profile-stats">
                                <div className="stat-item">
                                    <span className="stat-number">{orders.length}</span>
                                    <span className="stat-label">Всего заказов</span>
                                </div>
                                <div className="stat-item">
                                    <span className="stat-number">
                                        {orders.filter(order => order.status === 'completed').length}
                                    </span>
                                    <span className="stat-label">Завершено</span>
                                </div>
                                <div className="stat-item">
                                    <span className="stat-number">
                                        {orders.filter(order => order.status === 'in_progress' || order.status === 'new').length}
                                    </span>
                                    <span className="stat-label">В работе</span>
                                </div>
                            </div>
                        </div>
                    </div>
                )}

                {/* --- ЗАКАЗЫ --- */}
                {activeTab === 'orders' && (
                    <div className="profile-content">
                        <div className="orders-header">
                            <h3>История заказов</h3>
                            <button className="new-order-btn" onClick={handleCreateOrder}>
                                + Новый заказ
                            </button>
                        </div>

                        {orders.length === 0 ? (
                            <div className="empty-orders">
                                <p>У вас пока нет заказов</p>
                                <button className="cta-order-btn" onClick={handleCreateOrder}>
                                    Создать первый заказ
                                </button>
                            </div>
                        ) : (
                            <div className="orders-list">
                                {orders.map(order => (
                                    <div key={order.id} className="order-card" onClick={() => debugOrder(order)}>
                                        <div className="order-header">
                                            <span className="order-service">
                                                {'Заказ от '+ (
                                                    order.createdAt
                                                        ? new Date(order.createdAt).toLocaleDateString('ru-RU')
                                                        : new Date().toLocaleDateString('ru-RU')
                                                )}
</span>
                                            <span className={`order-status ${getStatusClass(order.status)}`}>
                                                {getStatusText(order.status)}
                                            </span>
                                        </div>
                                        <div className="order-details">
                                            <span className="order-price">{order.totalPrice || order.TotalPrice || 0} ₽</span>
                                            <span className="order-date">
                                                {new Date(order.createdAt || order.CreatedAt).toLocaleDateString('ru-RU')}
                                            </span>
                                        </div>
                                        <div className="order-info">
                                            <p><strong>Клиент:</strong> {order.customerName || order.CustomerName}</p>
                                            <p><strong>Телефон:</strong> {order.phone || order.Phone}</p>
                                        </div>
                                        <div className="order-actions">
                                            <button className="order-action-btn">Подробнее</button>
                                            <button className="order-action-btn">Написать</button>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                )}

                {/* --- НАСТРОЙКИ --- */}
                {activeTab === 'settings' && (
                    <div className="profile-content">
                        <div className="settings-section">
                            <h3>Уведомления</h3>
                            <div className="setting-item">
                                <div className="setting-info">
                                    <span className="setting-label">Уведомления о заказах</span>
                                    <span className="setting-description">Получать уведомления о статусе заказов</span>
                                </div>
                                <label className="switch">
                                    <input
                                        type="checkbox"
                                        checked={settings.notifications}
                                        onChange={() => handleSettingChange('notifications')}
                                    />
                                    <span className="slider"></span>
                                </label>
                            </div>
                        </div>

                        <div className="settings-section">
                            <h3>Аккаунт</h3>
                            <div className="account-actions">
                                <button className="account-btn change-password">
                                    Сменить пароль
                                </button>
                            </div>
                        </div>
                    </div>
                )}

                <div className="profile-footer">
                    <button className="logout-btn" onClick={onLogout}>
                        🚪 Выйти из аккаунта
                    </button>
                </div>
            </div>
        </div>
    );
}

export default ProfilePage;