import React from 'react';

function HomePage({ services, portfolio }) {
    return (
        <div>
            {/* Блок-герой с изображением */}
            <section className="hero">
                <div className="hero-content">
                    <h1 className="hero-title">Создаем стиль вместе с вами</h1>
                    <p className="hero-description">
                        Профессиональный пошив и ремонт одежды.
                        Индивидуальный подход к каждому клиенту.
                    </p>
                    <button className="cta-button">Посмотреть услуги</button>
                </div>
                <div className="hero-image">
                    <img src="/images/hero-bg.jpg" alt="Швейная мастерская" />
                </div>
            </section>

            {/* Список услуг */}
            <section id="services" className="services-section">
                <div className="container">
                    <h2 className="section-title">Наши услуги</h2>
                    <div className="services-grid">
                        {services.map(service => (
                            <div key={service.id} className="service-card">
                                <div className="service-preview">
                                    <img src={service.preview} alt={service.name} />
                                    <div className="service-overlay">
                                        <button className="service-btn">Подробнее</button>
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
                    <h2 className="section-title">Наше портфолио</h2>
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

            {/* Нижняя шапка */}
            <footer className="footer">
                <div className="container">
                    <div className="footer-content">
                        <div className="footer-section">
                            <h4>Atelier Studio</h4>
                            <p>Профессиональные услуги пошива и ремонта одежды</p>
                        </div>

                        <div className="footer-section">
                            <h4>Контакты</h4>
                            <p>+7 (999) 123-45-67</p>
                            <p>hello@atelier.ru</p>
                            <p>Москва, ул. Примерная, 123</p>
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
                        <p>&copy; 2025 Atelier Studio. Все права защищены.</p>
                    </div>
                </div>
            </footer>
        </div>
    );
}

export default HomePage;