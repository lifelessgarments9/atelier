import React, { useState } from 'react';
import './AuthPage.css';
import { useNavigate } from 'react-router-dom';

function AuthPage({onLogin }) {
    const navigate = useNavigate();
    const [activeTab, setActiveTab] = useState('login'); // register или login
    const [step, setStep] = useState('register'); // register → verify (только для регистрации)
    const [output, setOutput] = useState('');
    const [deepLink, setDeepLink] = useState('');

    const [registerData, setRegisterData] = useState({
        tg: '',
        pass: '',
        confirmPass: ''
    });

    const [verifyData, setVerifyData] = useState({
        tg: '',
        code: ''
    });

    const [loginData, setLoginData] = useState({
        tg: '',
        pass: ''
    });

    const API_URL = 'http://localhost:5041/api/auth';

    // ------------------ РЕГИСТРАЦИЯ ------------------
    const register = async () => {
        if (registerData.pass !== registerData.confirmPass) {
            setOutput('❌ Пароли не совпадают');
            return;
        }
        if (!registerData.tg) {
            setOutput('❌ Введите Telegram username (без @)');
            return;
        }

        try {
            const cleanUsername = registerData.tg.replace('@', '').trim();
            const res = await fetch(`${API_URL}/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    TgUsername: cleanUsername,
                    Password: registerData.pass
                })
            });

            const data = await res.json();
            if (!res.ok) throw new Error(data.message || 'Ошибка регистрации');

            // Сохраняем deep link и переходим к верификации
            if (data.deepLink) {
                setDeepLink(data.deepLink);
                setOutput(`✅ Регистрация начата! Перейдите по ссылке в Telegram.`);
            } else {
                setOutput('✅ Регистрация выполнена. Проверьте Telegram для получения кода.');
            }

            setVerifyData({ tg: cleanUsername, code: '' });
            setStep('verify');
        } catch (err) {
            setOutput(`❌ ${err.message}`);
        }
    };

    // ------------------ ВЕРИФИКАЦИЯ ------------------
    const verify = async () => {
        if (!verifyData.code) {
            setOutput('Введите код из Telegram');
            return;
        }

        try {
            const res = await fetch(`${API_URL}/verify`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    TgUsername: verifyData.tg,
                    Code: verifyData.code
                })
            });

            const data = await res.json();
            if (!res.ok) throw new Error(data.message || 'Ошибка подтверждения');

            setOutput(`✅ Регистрация завершена! Добро пожаловать, ${data.username}!`);
            setTimeout(() => {
                onLogin(data.username); // передаем имя пользователя
            }, 1500);
        } catch (err) {
            setOutput(`❌ ${err.message}`);
        }
    };

    // ------------------ ВХОД ------------------
    const login = async () => {
        if (!loginData.tg || !loginData.pass) {
            setOutput('Введите Telegram username и пароль');
            return;
        }

        try {
            const cleanUsername = loginData.tg.replace('@', '').trim();
            const res = await fetch(`${API_URL}/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    TgUsername: cleanUsername,
                    Password: loginData.pass
                })
            });

            const data = await res.json();
            if (!res.ok) throw new Error(data.message || 'Ошибка входа');

            setOutput(`✅ Вход выполнен! Добро пожаловать, ${data.username}!`);
            setTimeout(() => {
                onLogin(data.username);
            }, 1500);
        } catch (err) {
            setOutput(`❌ ${err.message}`);
        }
    };

    // ------------------ UI ------------------
    return (
        <div className="auth-page">
            <div className="auth-container">
                <button className="back-btn" onClick={() => navigate('/')}>
                    ← Назад на главную
                </button>

                <div className="auth-tabs">
                    <button
                        className={`tab ${activeTab === 'login' ? 'active' : ''}`}
                        onClick={() => setActiveTab('login')}
                    >
                        Вход
                    </button>
                    <button
                        className={`tab ${activeTab === 'register' ? 'active' : ''}`}
                        onClick={() => setActiveTab('register')}
                    >
                        Регистрация
                    </button>
                </div>

                {/* --- РЕГИСТРАЦИЯ --- */}
                {activeTab === 'register' && step === 'register' && (
                    <div className="auth-form">
                        <h3>Создание аккаунта</h3>

                        <div className="input-group">
                            <input
                                placeholder="Telegram username (без @)"
                                value={registerData.tg}
                                onChange={(e) => setRegisterData({ ...registerData, tg: e.target.value })}
                                required
                            />
                        </div>

                        <div className="input-group">
                            <input
                                placeholder="Пароль"
                                type="password"
                                value={registerData.pass}
                                onChange={(e) => setRegisterData({ ...registerData, pass: e.target.value })}
                                required
                            />
                        </div>

                        <div className="input-group">
                            <input
                                placeholder="Подтвердите пароль"
                                type="password"
                                value={registerData.confirmPass}
                                onChange={(e) => setRegisterData({ ...registerData, confirmPass: e.target.value })}
                                required
                            />
                        </div>

                        <button className="auth-submit-btn" onClick={register}>
                            Зарегистрироваться
                        </button>
                    </div>
                )}

                {/* --- ПОДТВЕРЖДЕНИЕ КОДА (только для регистрации) --- */}
                {activeTab === 'register' && step === 'verify' && (
                    <div className="auth-form">
                        <h3>Введите код подтверждения</h3>
                        <p>Мы отправили код на Telegram @{verifyData.tg}</p>

                        {deepLink && (
                            <div className="deep-link-section">
                                <p><strong>Шаг 1:</strong> Перейдите по ссылке в Telegram:</p>
                                <div className="deep-link-container">
                                    <a href={deepLink} target="_blank" rel="noopener noreferrer" className="deep-link">
                                        🔗 Нажмите чтобы открыть Telegram
                                    </a>
                                    <button
                                        className="copy-btn"
                                        onClick={() => navigator.clipboard.writeText(deepLink)}
                                        title="Скопировать ссылку"
                                    >
                                        📋
                                    </button>
                                </div>
                                <p><strong>Шаг 2:</strong> Бот отправит вам 6-значный код</p>
                                <p><strong>Шаг 3:</strong> Введите код ниже:</p>
                            </div>
                        )}

                        <div className="input-group">
                            <input
                                placeholder="Введите 6-значный код из Telegram"
                                value={verifyData.code}
                                onChange={(e) => setVerifyData({ ...verifyData, code: e.target.value })}
                                maxLength={6}
                                required
                            />
                        </div>

                        <button className="auth-submit-btn" onClick={verify}>
                            Завершить регистрацию
                        </button>

                        <button className="auth-link-btn" onClick={() => setStep('register')}>
                            ← Назад к регистрации
                        </button>
                    </div>
                )}

                {/* --- ВХОД --- */}
                {activeTab === 'login' && (
                    <div className="auth-form">
                        <h3>Вход в аккаунт</h3>

                        <div className="input-group">
                            <input
                                placeholder="Telegram username (без @)"
                                value={loginData.tg}
                                onChange={(e) => setLoginData({ ...loginData, tg: e.target.value })}
                                required
                            />
                        </div>

                        <div className="input-group">
                            <input
                                placeholder="Пароль"
                                type="password"
                                value={loginData.pass}
                                onChange={(e) => setLoginData({ ...loginData, pass: e.target.value })}
                                required
                            />
                        </div>

                        <button className="auth-submit-btn" onClick={login}>
                            Войти
                        </button>
                    </div>
                )}

                {output && <pre className="output">{output}</pre>}
            </div>
        </div>
    );
}

export default AuthPage;