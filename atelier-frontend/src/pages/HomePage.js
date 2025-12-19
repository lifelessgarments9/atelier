import React from 'react';
import { useNavigate } from 'react-router-dom';
import img1 from 'C:/tmp/аип/Solution/atelier-frontend/src/images/IMG_5789.jpg';



function HomePage({ services, portfolio, onNavClick }) {
    const navigate = useNavigate();
    return (
        <div>
            {/* Блок с изображением */}
            <section className="hero" id="hero">
                <div className="hero-content">
                    <h1 className="hero-title"></h1>
                    <p className="hero-description">
                        текст1
                    </p>
                </div>

                <div style={{ width: '100%', textAlign: 'center' }}>
                    <div className="hero-image">
                        <img src={img1} alt="лого"/>
                    </div>
                    <button
                        className="service-btn"
                        onClick={() => onNavClick('services')}
                        style={{ marginTop: '20px' }}
                    >
                        Посмотреть услуги
                    </button>
                </div>
            </section>

            {/* Список услуг */}
            <section id="services" className="services-section">
                <div className="container" style={{ position: 'relative', textAlign: 'center' }}>
                    {/* Контейнер для заголовка и изображения */}
                    <div style={{ position: 'relative', marginBottom: '40px' }}>
                        {/* Изображение поверх заголовка */}
                        <img
                            src="/ui/11.png"
                            alt="усл"
                            style={{
                                width: '290px',
                                height: '290px',
                                position: 'absolute',
                                top: '-290px',      
                                left: '50%',
                                transform: 'translateX(-50%)',
                                zIndex: 2
                            }}
                        />

                        {/* Заголовок */}
                        <h2
                            className="section-title"
                            style={{
                                position: 'relative',
                                zIndex: 1,
                                marginTop: '190px'  // Отступ для изображения сверху
                            }}
                        >
                            
                        </h2>
                    </div>
                    <div className="services-grid">
                        {services.map(service => (
                            <div key={service.id} className="service-card">
                                <div className="service-preview">
                                    <img src={service.preview} alt={service.name} /> 
                                    <div className="service-overlay">
                                        <button className="service-btn" onClick={() =>navigate(`/model/${service.id}`)}>Подробнее</button> 
                                    </div>
                                </div>
                                <h3 className="service-name">{service.name}</h3>
                                <p className="service-description">{service.description}</p>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            {/* Разделитель */}
            <div className="divider"></div>

            {/* Портфолио */}
            <section id="portfolio" className="portfolio-section">
                <div className="container">
                    <div style={{ position: 'relative', marginBottom: '40px' }}>
                        {/* Изображение поверх заголовка */}
                        <img
                            src="/ui/1з.png"
                            alt="усл"
                            style={{
                                width: '350px',
                                height: '350px',
                                position: 'absolute',
                                top: '-270px',
                                left: '50%',
                                transform: 'translateX(-50%)',
                                pointerEvents: 'none',
                                userSelect: 'none',
                                zIndex: 2
                            }}
                        />

                        {/* Заголовок */}
                        <h2
                            className="section-title"
                            style={{
                                position: 'relative',
                                zIndex: 1,
                                marginTop: '190px'  // Отступ для изображения сверху
                            }}
                        >

                        </h2>
                    </div>
                    <div className="portfolio-grid">
                        {portfolio.map(item => (
                            <div key={item.id} className="portfolio-card">
                                <div className="portfolio-preview">
                                    <img src={item.preview} alt={item.name} />
                                    <div className="portfolio-overlay">
                                        <span className="portfolio-name">{item.name}</span>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            {/* Подвал */}
            <footer id="footer" className="footer">
                <div className="container">
                    <div className="footer-content">
                        <div className="footer-section">
                            <h4>Atelier Studio</h4>
                            <p>Профессиональные услуги пошива и ремонта одежды</p>
                        </div>

                        <div className="footer-section">
                            <h4>Контакты</h4>
                            <p>+7 (799) 268-15-45</p>
                            <p>hello@atelier.ru</p>
                            <p>Москва, ул. Полярная, 11б</p>
                        </div>

                        <div className="footer-section">
                            <h4>Часы работы</h4>
                            <p>Пн-Пт: 9:00 - 20:00</p>
                            <p>Сб-Вс: 10:00 - 18:00</p>
                        </div>

                        <div className="footer-section">
                            <h4>Соцсети</h4>
                            <div className="social-links">
                                <a href="#">Telegram</a>
                                <a href="#">Instagram</a>
                                <a href="#">VK</a>
                            </div>
                        </div>
                    </div>

                    <div className="footer-bottom">
                        <p>&copy; ателье</p>
                    </div>
                </div>
            </footer>
        </div>
    );
}

export default HomePage;