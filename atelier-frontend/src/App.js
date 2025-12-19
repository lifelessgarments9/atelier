import './App.css';
import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate, useLocation } from 'react-router-dom';
import AuthPage from './pages/AuthPage';
import ProfilePage from './pages/ProfilePage';
import NewOrderPage from './pages/NewOrderPage';
import HomePage from './pages/HomePage';
import ModelPage from './pages/ModelPage';
import img1 from 'C:/tmp/аип/Solution/atelier-frontend/src/images/IMG_5789.jpg';

function AppContent() {
  const [currentUser, setCurrentUser] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();

  const services = [
    { id: 1, name: 'Услуга_1', description: 'Описание_1', price: 5000, preview: img1 }, 
    { id: 2, name: 'Услуга_2', description: 'Описание_2', price: 1500, preview: img1 },
    { id: 3, name: 'Услуга_3', description: 'Описание_3', price: 3000, preview: img1 },
    { id: 4, name: 'Услуга_4', description: 'Описание_4', price: 7000, preview: img1 }
  ];

  const portfolio = [
    { id: 1, name: 'Вечернее платье', preview: '/ui/preview/8.png' },
    { id: 2, name: 'Костюм', preview: '/ui/preview/9.png' },
    { id: 3, name: 'Ремонт куртки', preview: '/ui/preview/10.png' },
    { id: 4, name: 'Детская одежда', preview: '/ui/preview/11.png' }
  ];

  const handleLogin = (username) => {
    setCurrentUser(username);
    navigate('/');
  };

  const handleLogout = () => {
    setCurrentUser(null);
    navigate('/');
  };

  const handleNavClick = (sectionId) => {
    if (location.pathname === '/') {
      const element = document.getElementById(sectionId);
      if (element) {
        element.scrollIntoView({ behavior: 'smooth' });
      }
    } else {
      // Если на другой странице, переходим на главную скроллим
      navigate('/');
      setTimeout(() => {
        const element = document.getElementById(sectionId);
        if (element) {
          element.scrollIntoView({ behavior: 'smooth' });
        }
      }, 100);
    }
  };

  return (
      <div className="app">
        {/* Header отображается на всех страницах */}
        <Header
            currentUser={currentUser}
            onLogout={handleLogout}
            onNavClick={handleNavClick}
        />

        <Routes>
          <Route path="/" element={
            <HomePage
                services={services}
                portfolio={portfolio}
                onNavClick={handleNavClick}
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
          <Route path="/model/:modelId" element={
            <ModelPage
                services={services}
                currentUser={currentUser}
            />
          } />
        </Routes>
      </div>
  );
}

function Header({ currentUser, onLogout, onNavClick }) {
  const navigate = useNavigate();

  return (
      <header className="header">
        <div className="header-content">
          <div className="logo-section">
            <img src="/anim/a.png" alt="Atelier Logo" className="logo" 
             style={{
                width: '120px', 
                height: 'auto',
             }}/>
              <div className="logo-section" style={{ position: 'relative', display: 'inline-flex' }}>
                  {/* Изображение поверх текста */}
                  <img
                      src="/ui/9.png"
                      alt="Atelier Logo"
                      style={{
                          width: '300px',
                          height: 'auto',
                          position: 'absolute',
                          left: '50%',
                          top: '50%',
                          transform: 'translate(-8%, -79%)',
                          zIndex: 2
                      }}
                  />
              </div>
          </div>

          <nav className="nav">
            <button
                className="nav-link"
                onClick={() => onNavClick('hero')}
                style={{
                  background: 'none',
                  border: 'none',
                  padding: '0px 4px',
                  cursor: 'pointer',
                    minWidth: '40px',
                    height: '60px'
                }}
            >
              <img
                  src="/ui/13.png"
                  alt="О нас"
                  style={{
                    width: '100px',      
                    height: '80px',
                    display: 'block'
                  }}
              />
            </button>
            <button
                className="nav-link"
                onClick={() => onNavClick('services')}
                style={{
                  background: 'none',
                  border: 'none',
                    padding: '0px 4px',
                    cursor: 'pointer',
                    minWidth: '40px',
                    height: '30px'
                }}
            >
                <img
                    src="/ui/15.png"
                    alt="услуги"
                    style={{
                        width: '130px',
                        height: '60px',
                        display: 'block'
                    }}
                />
            </button>
            <button
                className="nav-link"
                onClick={() => onNavClick('portfolio')}
                style={{
                  background: 'none',
                  border: 'none',
                    padding: '0px 4px',
                    cursor: 'pointer',
                    minWidth: '40px',
                    height: '60px'
                }}
            >
                <img
                    src="/ui/14.png"
                    alt="портфолио"
                    style={{
                        width: '190px',
                        height: '80px',
                        display: 'block'
                    }}
                />
            </button>
            <button
                className="nav-link"
                onClick={() => onNavClick('footer')}
                style={{
                  background: 'none',
                  border: 'none',
                    padding: '0px 4px',
                    cursor: 'pointer',
                    minWidth: '40px',
                    height: '60px'
                }}
            >
                <img
                    src="/ui/12.png"
                    alt="конт"
                    style={{
                        width: '150px',
                        height: '70px',
                        display: 'block'
                    }}
                />
            </button>
          </nav>

          <div className="header-actions">
            {currentUser ? (
                <div className="user-profile">
                  <span className="username-overlay"
                        style={{
                            position: 'absolute',
                            top: '84%',
                            left: '88.5%',
                            transform: 'translate(-50%, -50%)',
                            color: '#495057',
                            zIndex: 2,
                            maxWidth: '70px',
                        }}>{currentUser}</span>
                  <button
                      className="profile-btn"
                      onClick={() => navigate('/profile')}
                      style={{
                          position: 'relative',
                          background: 'none',
                          border: 'none',
                          padding: '0px 4px',
                          cursor: 'pointer',
                          minWidth: '40px',
                          height: '80px'
                      }}
                  >
                      <img
                          src="/ui/ава.png"
                          alt="ава"
                          style={{
                              width: '80px',
                              height: '80px',
                              display: 'block'
                          }}
                      />
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

function App() {
  return (
      <Router>
        <AppContent />
      </Router>
  );
}

export default App;