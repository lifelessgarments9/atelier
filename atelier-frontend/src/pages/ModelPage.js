import React, { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import * as THREE from 'three';
import { FBXLoader } from 'three/examples/jsm/loaders/FBXLoader';

function ModelPage({ services }) {
    const { modelId } = useParams();
    const navigate = useNavigate();
    const canvasRef = useRef(null);

    // Ссылки на Three.js объекты
    const sceneRef = useRef(null);
    const cameraRef = useRef(null);
    const rendererRef = useRef(null);
    const modelRef = useRef(null);
    const controlsRef = useRef({ x: 0, y: 0 });

    const [dragging, setDragging] = useState(false);
    const [lastPos, setLastPos] = useState({ x: 0, y: 0 });
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    // Соответствие ID услуг и моделей
    const modelFiles = {
        1: '/models/dress2.fbx',
        2: '/models/dress2.fbx',
        3: '/models/dress2.fbx',
        4: '/models/dress2.fbx'
    };

    useEffect(() => {
        const service = services.find(s => s.id === parseInt(modelId));
        if (!service || !canvasRef.current) return;

        // Инициализация Three.js
        const scene = new THREE.Scene();
        sceneRef.current = scene;

        const camera = new THREE.PerspectiveCamera(
            45,
            canvasRef.current.clientWidth / canvasRef.current.clientHeight,
            0.1,
            1000
        );
        camera.position.z = 5;
        cameraRef.current = camera;

        const renderer = new THREE.WebGLRenderer({
            canvas: canvasRef.current,
            antialias: true,
            alpha: true
        });
        renderer.setSize(canvasRef.current.clientWidth, canvasRef.current.clientHeight);
        renderer.setClearColor(0xf8f9fa, 1);
        rendererRef.current = renderer;

        // Освещение
        const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
        scene.add(ambientLight);

        const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
        directionalLight.position.set(1, 1, 1);
        scene.add(directionalLight);

        // Загрузка FBX модели
        const loader = new FBXLoader();
        const modelPath = modelFiles[modelId] || '/models/dress2.fbx';

        const frames = [
            "/anim/1.png",
            "/anim/2.png",
            "/anim/3.png",
            "/anim/4.png",
            "/anim/5.png",
        ];



        setLoading(true);
        setError(null);

        loader.load(
            // URL модели
            modelPath,

            // Успешная загрузка
            (fbx) => {
                // Удаляем предыдущую модель
                if (modelRef.current) {
                    scene.remove(modelRef.current);
                }

                // Настраиваем новую модель
                fbx.scale.setScalar(0.01); // Масштабируем под сцену
                fbx.position.set(0, 0, 0);
                fbx.traverse((child) => {
                    if (child.isMesh) {
                        child.material = new THREE.MeshPhongMaterial({
                            color: 0x4285f4,
                            shininess: 100
                        });
                    }
                });

                scene.add(fbx);
                modelRef.current = fbx;
                setLoading(false);

                // Центрируем камеру на модели
                const box = new THREE.Box3().setFromObject(fbx);
                const center = box.getCenter(new THREE.Vector3());
                const size = box.getSize(new THREE.Vector3());

                camera.position.set(
                    center.x,
                    center.y,
                    center.z + Math.max(size.x, size.y, size.z) * 1.5
                );
                camera.lookAt(center);

                renderer.render(scene, camera);
            },

            // Прогресс загрузки
            (xhr) => {
                console.log(`${(xhr.loaded / xhr.total) * 100}% загружено`);
            },

            // Ошибка загрузки
            (err) => {
                console.error('Ошибка загрузки FBX:', err);
                setError('Не удалось загрузить 3D модель');
                setLoading(false);

            }
        );

        // Функция рендеринга
        const render = () => {
            if (modelRef.current) {
                modelRef.current.rotation.x = controlsRef.current.x;
                modelRef.current.rotation.y = controlsRef.current.y;
            }
            renderer.render(scene, camera);
        };

        // Анимация
        const animate = () => {
            requestAnimationFrame(animate);
            render();
        };
        animate();

        // Обработка изменения размера окна
        const handleResize = () => {
            if (!canvasRef.current || !cameraRef.current || !rendererRef.current) return;

            const width = canvasRef.current.clientWidth;
            const height = canvasRef.current.clientHeight;

            cameraRef.current.aspect = width / height;
            cameraRef.current.updateProjectionMatrix();
            rendererRef.current.setSize(width, height, false);
            render();
        };

        window.addEventListener('resize', handleResize);

        // Очистка
        return () => {
            window.removeEventListener('resize', handleResize);
            if (rendererRef.current) {
                rendererRef.current.dispose();
            }
            if (modelRef.current) {
                scene.remove(modelRef.current);
            }
        };
    }, [modelId, services]);

    // Обработка вращения мышью
    const handleMouseDown = (e) => {
        setDragging(true);
        setLastPos({ x: e.clientX, y: e.clientY });
    };

    const handleMouseMove = (e) => {
        if (!dragging) return;

        const deltaX = e.clientX - lastPos.x;
        const deltaY = e.clientY - lastPos.y;

        controlsRef.current = {
            x: controlsRef.current.x, 
            y: controlsRef.current.y + deltaX * 0.01 
        };

        setLastPos({ x: e.clientX, y: e.clientY });
    };

    const handleMouseUp = () => {
        setDragging(false);
    };

    const service = services.find(s => s.id === parseInt(modelId));
    if (!service) return <div style={styles.error}>Услуга не найдена</div>;

    return (
        <div style={styles.page}>
            {/* Информация */}
            <div style={styles.info}>


                <h1 style={styles.title}>{service.name}</h1>
                <div style={styles.price}>{service.price} ₽</div>
                <p style={styles.desc}>{service.description}</p>

            </div>

            {/* 3D Модель */}
            <div
                style={{
                    ...styles.viewer,
                    cursor: dragging ? 'grabbing' : 'grab'
                }}
                onMouseDown={handleMouseDown}
                onMouseMove={handleMouseMove}
                onMouseUp={handleMouseUp}
                onMouseLeave={handleMouseUp}
            >
                <canvas ref={canvasRef} style={{ width: '100%', height: '100%' }} />

                {loading && (
                    <div style={styles.loading}>
                        Загрузка 3D модели... 
                    </div>
                )}

                {error && (
                    <div style={styles.errorOverlay}>
                        {error}
                    </div>
                )}
            </div>
        </div>
    );
}



const styles = {
    page: {
        display: 'flex',
        height: '100vh',
        overflow: 'hidden'
    },
    info: {
        flex: '0 0 350px',
        padding: '30px',
        background: '#fff',
        borderRight: '1px solid #eaeaea',
        overflowY: 'auto'
    },

    title: {
        fontSize: '28px',
        margin: '0 0 10px 0',
        color: '#212529'
    },
    price: {
        fontSize: '24px',
        color: '#28a745',
        fontWeight: '600',
        marginBottom: '20px'
    },
    desc: {
        color: '#666',
        lineHeight: '1.6',
        fontSize: '16px',
        marginBottom: '20px'
    },
    features: {
        marginTop: '20px'
    },
    list: {
        listStyle: 'none',
        padding: '0',
        margin: '0'
    },
    listItem: {
        padding: '8px 0',
        color: '#495057'
    },
    viewer: {
        flex: '1',
        background: '#f8f9fa',
        position: 'relative'
    },

    loading: {
        position: 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        background: 'rgba(255,255,255,0.9)',
        padding: '20px 30px',
        borderRadius: '8px',
        color: '#666'
    },
    errorOverlay: {
        position: 'absolute',
        top: '20px',
        left: '20px',
        right: '20px',
        background: '#f8d7da',
        color: '#721c24',
        padding: '15px',
        borderRadius: '6px',
        textAlign: 'center'
    },
    error: {
        padding: '40px',
        textAlign: 'center',
        color: '#dc3545'
    }
};

// Добавляем стили для элементов списка
styles.listItem = {
    padding: '8px 0',
    color: '#495057',
    position: 'relative',
    paddingLeft: '24px',
    borderBottom: '1px solid #eee'
};

styles.listItem.before = {
    content: '"✓"',
    position: 'absolute',
    left: '0',
    color: '#28a745'
};

export default ModelPage;