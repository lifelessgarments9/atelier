import './App.css';
import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate, useLocation } from 'react-router-dom';
import AuthPage from './pages/AuthPage';
import ProfilePage from './pages/ProfilePage';
import NewOrderPage from './pages/NewOrderPage';
import HomePage from './pages/HomePage'; // создадим этот компонент

// Главный компонент приложения с маршрутизацией
function AppContent() {
  const [currentUser, setCurrentUser] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();

  // Данные для услуг
  const services = [
    { id: 1, name: 'Пошив одежды', description: 'Индивидуальный пошив', price: 5000 },
    { id: 2, name: 'Ремонт одежды', description: 'Качественный ремонт', price: 1500 },
    { id: 3, name: 'Ателье', description: 'Профессиональное ателье', price: 3000 },
    { id: 4, name: '3D моделирование', description: 'Создание 3D моделей', price: 7000 }
  ];

  const portfolio = [
    { id: 1, name: 'Вечернее платье', preview: '/images/dress-portfolio.jpg' },
    { id: 2, name: 'Костюм', preview: '/images/suit-portfolio.jpg' },
    { id: 3, name: 'Ремонт куртки', preview: '/images/jacket-portfolio.jpg' },
    { id: 4, name: 'Детская одежда', preview: '/images/kids-portfolio.jpg' }
  ];

  const handleLogin = (username) => {
    setCurrentUser(username);
    navigate('/'); // возвращаем на главную после входа
  };

  const handleLogout = () => {
    setCurrentUser(null);
    navigate('/');
  };

  const handleOrderCreated = () => {
    navigate('/'); // возвращаем на главную после создания заказа
  };

  // Общие пропсы для Header
  const headerProps = {
    currentUser,
    onLogout: handleLogout
  };

  return (
      <div className="app">
        {/* Header отображается на всех страницах */}
        <Header {...headerProps} />

        <Routes>
          <Route path="/" element={
            <HomePage
                services={services}
                portfolio={portfolio}
            />
          } />
          <Route path="/auth" element={
            <AuthPage onLogin={handleLogin} />
          } />
          <Route path="/profile" element={
            <ProfilePage
                user={currentUser}
                onLogout={handleLogout}
            />
          } />
          <Route path="/neworder" element={
            <NewOrderPage
                services={services}
                currentUser={currentUser}
            />
          } />
        </Routes>
      </div>
  );
}

// Header компонент
function Header({ currentUser, onLogout }) {
  const navigate = useNavigate();

  return (
      <header className="header">
        <div className="header-content">
          <div className="logo-section">
            <img src="/images/logo.png" alt="Atelier Logo" className="logo" />
            <span className="logo-text">Atelier Studio</span>
          </div>

          <nav className="nav">
            <a href="#services" onClick={(e) => { e.preventDefault(); navigate('/#services'); }}>
              Услуги
            </a>
            <a href="#portfolio" onClick={(e) => { e.preventDefault(); navigate('/#portfolio'); }}>
              Портфолио
            </a>
            <a href="#about" onClick={(e) => { e.preventDefault(); navigate('/#about'); }}>
              О нас
            </a>
            <a href="#contacts" onClick={(e) => { e.preventDefault(); navigate('/#contacts'); }}>
              Контакты
            </a>
          </nav>

          <div className="header-actions">
            {currentUser ? (
                <div className="user-profile">
                  <div className="profile-icon">👤</div>
                  <span className="username">{currentUser}</span>
                  <button
                      className="profile-btn"
                      onClick={() => navigate('/profile')}
                  >
                    Профиль
                  </button>
                  <button className="logout-btn" onClick={onLogout}>
                    Выйти
                  </button>
                </div>
            ) : (
                <button
                    className="auth-btn"
                    onClick={() => navigate('/auth')}
                >
                  Войти
                </button>
            )}
          </div>
        </div>
      </header>
  );
}

// Главный компонент с Router
function App() {
  return (
      <Router>
        <AppContent />
      </Router>
  );
}

export default App;

