--
-- PostgreSQL database dump
--

\restrict EBfNDfgpVpp678ZC1MJJiwFdlym2w8kpf0zebzDd6J5psS8oGJ5jORTmU6Aefy8

-- Dumped from database version 17.6
-- Dumped by pg_dump version 17.6

-- Started on 2025-10-24 11:55:20

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 222 (class 1259 OID 17321)
-- Name: orders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orders (
    id integer NOT NULL,
    userid integer,
    customername character varying(255) NOT NULL,
    tgusername character varying(255) NOT NULL,
    phone character varying(20) NOT NULL,
    serviceids integer[] NOT NULL,
    servicenames text[] NOT NULL,
    totalprice numeric(10,2) NOT NULL,
    status character varying(50) DEFAULT 'New'::character varying,
    createdat timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updatedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    doneat timestamp without time zone,
    CONSTRAINT orders_status_check CHECK (((status)::text = ANY ((ARRAY['New'::character varying, 'InProgress'::character varying, 'Ready'::character varying, 'Issued'::character varying])::text[])))
);


ALTER TABLE public.orders OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 17320)
-- Name: orders_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.orders_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.orders_id_seq OWNER TO postgres;

--
-- TOC entry 4944 (class 0 OID 0)
-- Dependencies: 221
-- Name: orders_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.orders_id_seq OWNED BY public.orders.id;


--
-- TOC entry 223 (class 1259 OID 17341)
-- Name: orderservices; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orderservices (
    orderid integer NOT NULL,
    serviceid integer NOT NULL,
    quantity integer DEFAULT 1
);


ALTER TABLE public.orderservices OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 17314)
-- Name: services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.services (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    price numeric(10,2) NOT NULL
);


ALTER TABLE public.services OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 17313)
-- Name: services_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.services_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.services_id_seq OWNER TO postgres;

--
-- TOC entry 4945 (class 0 OID 0)
-- Dependencies: 219
-- Name: services_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.services_id_seq OWNED BY public.services.id;


--
-- TOC entry 218 (class 1259 OID 17298)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    tgusername character varying(255) NOT NULL,
    passwordhash character varying(255) NOT NULL,
    firstname character varying(100) NOT NULL,
    lastname character varying(100) NOT NULL,
    phone character varying(20),
    createdat timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updatedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    isactive boolean DEFAULT true,
    role character varying(50) DEFAULT 'Customer'::character varying
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 17297)
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_id_seq OWNER TO postgres;

--
-- TOC entry 4946 (class 0 OID 0)
-- Dependencies: 217
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- TOC entry 4762 (class 2604 OID 17324)
-- Name: orders id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders ALTER COLUMN id SET DEFAULT nextval('public.orders_id_seq'::regclass);


--
-- TOC entry 4761 (class 2604 OID 17317)
-- Name: services id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services ALTER COLUMN id SET DEFAULT nextval('public.services_id_seq'::regclass);


--
-- TOC entry 4756 (class 2604 OID 17301)
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- TOC entry 4937 (class 0 OID 17321)
-- Dependencies: 222
-- Data for Name: orders; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.orders (id, userid, customername, tgusername, phone, serviceids, servicenames, totalprice, status, createdat, updatedat, doneat) FROM stdin;
\.


--
-- TOC entry 4938 (class 0 OID 17341)
-- Dependencies: 223
-- Data for Name: orderservices; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.orderservices (orderid, serviceid, quantity) FROM stdin;
\.


--
-- TOC entry 4935 (class 0 OID 17314)
-- Dependencies: 220
-- Data for Name: services; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.services (id, name, price) FROM stdin;
\.


--
-- TOC entry 4933 (class 0 OID 17298)
-- Dependencies: 218
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, tgusername, passwordhash, firstname, lastname, phone, createdat, updatedat, isactive, role) FROM stdin;
\.


--
-- TOC entry 4947 (class 0 OID 0)
-- Dependencies: 221
-- Name: orders_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.orders_id_seq', 5, true);


--
-- TOC entry 4948 (class 0 OID 0)
-- Dependencies: 219
-- Name: services_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.services_id_seq', 1, false);


--
-- TOC entry 4949 (class 0 OID 0)
-- Dependencies: 217
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 1, false);


--
-- TOC entry 4779 (class 2606 OID 17332)
-- Name: orders orders_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT orders_pkey PRIMARY KEY (id);


--
-- TOC entry 4783 (class 2606 OID 17346)
-- Name: orderservices orderservices_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orderservices
    ADD CONSTRAINT orderservices_pkey PRIMARY KEY (orderid, serviceid);


--
-- TOC entry 4774 (class 2606 OID 17319)
-- Name: services services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services
    ADD CONSTRAINT services_pkey PRIMARY KEY (id);


--
-- TOC entry 4770 (class 2606 OID 17309)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- TOC entry 4772 (class 2606 OID 17311)
-- Name: users users_tgusername_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_tgusername_key UNIQUE (tgusername);


--
-- TOC entry 4775 (class 1259 OID 17340)
-- Name: idx_orders_createdat; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orders_createdat ON public.orders USING btree (createdat);


--
-- TOC entry 4776 (class 1259 OID 17339)
-- Name: idx_orders_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orders_status ON public.orders USING btree (status);


--
-- TOC entry 4777 (class 1259 OID 17338)
-- Name: idx_orders_userid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orders_userid ON public.orders USING btree (userid);


--
-- TOC entry 4780 (class 1259 OID 17357)
-- Name: idx_orderservices_orderid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orderservices_orderid ON public.orderservices USING btree (orderid);


--
-- TOC entry 4781 (class 1259 OID 17358)
-- Name: idx_orderservices_serviceid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orderservices_serviceid ON public.orderservices USING btree (serviceid);


--
-- TOC entry 4768 (class 1259 OID 17312)
-- Name: idx_users_tgusername; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_users_tgusername ON public.users USING btree (tgusername);


--
-- TOC entry 4784 (class 2606 OID 17333)
-- Name: orders orders_userid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT orders_userid_fkey FOREIGN KEY (userid) REFERENCES public.users(id) ON DELETE SET NULL;


--
-- TOC entry 4785 (class 2606 OID 17347)
-- Name: orderservices orderservices_orderid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orderservices
    ADD CONSTRAINT orderservices_orderid_fkey FOREIGN KEY (orderid) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4786 (class 2606 OID 17352)
-- Name: orderservices orderservices_serviceid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orderservices
    ADD CONSTRAINT orderservices_serviceid_fkey FOREIGN KEY (serviceid) REFERENCES public.services(id) ON DELETE CASCADE;


-- Completed on 2025-10-24 11:55:20

--
-- PostgreSQL database dump complete
--

\unrestrict EBfNDfgpVpp678ZC1MJJiwFdlym2w8kpf0zebzDd6J5psS8oGJ5jORTmU6Aefy8

