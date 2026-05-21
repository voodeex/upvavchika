--
-- PostgreSQL database dump
--

\restrict x95kI9jH7gRv7QSMjiZ5PtuOydD7dTc6fmkgI0FxZ0OBno7UTRIvKifa5HLNtTL

-- Dumped from database version 18.0
-- Dumped by pg_dump version 18.0

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
-- Name: book_genre; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.book_genre (
    book_id integer NOT NULL,
    genre_id integer NOT NULL
);


ALTER TABLE public.book_genre OWNER TO postgres;

--
-- Name: books; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.books (
    id integer NOT NULL,
    title character varying(200) NOT NULL,
    description text,
    cover_path character varying(500),
    content text,
    author_id integer NOT NULL,
    is_frozen boolean DEFAULT false NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.books OWNER TO postgres;

--
-- Name: books_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.books_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.books_id_seq OWNER TO postgres;

--
-- Name: books_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.books_id_seq OWNED BY public.books.id;


--
-- Name: complaints; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.complaints (
    id integer NOT NULL,
    user_id integer NOT NULL,
    book_id integer,
    review_id integer,
    reason text NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT complaints_check CHECK (((((book_id IS NOT NULL))::integer + ((review_id IS NOT NULL))::integer) = 1))
);


ALTER TABLE public.complaints OWNER TO postgres;

--
-- Name: complaints_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.complaints_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.complaints_id_seq OWNER TO postgres;

--
-- Name: complaints_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.complaints_id_seq OWNED BY public.complaints.id;


--
-- Name: genres; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.genres (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    description text
);


ALTER TABLE public.genres OWNER TO postgres;

--
-- Name: genres_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.genres_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.genres_id_seq OWNER TO postgres;

--
-- Name: genres_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.genres_id_seq OWNED BY public.genres.id;


--
-- Name: reading_lists; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reading_lists (
    id integer NOT NULL,
    user_id integer NOT NULL,
    book_id integer NOT NULL,
    status_id integer NOT NULL,
    added_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.reading_lists OWNER TO postgres;

--
-- Name: reading_lists_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.reading_lists_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.reading_lists_id_seq OWNER TO postgres;

--
-- Name: reading_lists_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.reading_lists_id_seq OWNED BY public.reading_lists.id;


--
-- Name: reading_statuses; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reading_statuses (
    id integer NOT NULL,
    name character varying(50) NOT NULL
);


ALTER TABLE public.reading_statuses OWNER TO postgres;

--
-- Name: reading_statuses_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.reading_statuses_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.reading_statuses_id_seq OWNER TO postgres;

--
-- Name: reading_statuses_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.reading_statuses_id_seq OWNED BY public.reading_statuses.id;


--
-- Name: reviews; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reviews (
    id integer NOT NULL,
    user_id integer NOT NULL,
    book_id integer NOT NULL,
    text text,
    rating integer NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT reviews_rating_check CHECK (((rating >= 1) AND (rating <= 10)))
);


ALTER TABLE public.reviews OWNER TO postgres;

--
-- Name: reviews_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.reviews_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.reviews_id_seq OWNER TO postgres;

--
-- Name: reviews_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.reviews_id_seq OWNED BY public.reviews.id;


--
-- Name: role_requests; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.role_requests (
    id integer NOT NULL,
    user_id integer NOT NULL,
    role_id integer NOT NULL,
    reason text,
    status character varying(20) DEFAULT 'pending'::character varying NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT role_requests_status_check CHECK (((status)::text = ANY ((ARRAY['pending'::character varying, 'approved'::character varying, 'rejected'::character varying])::text[])))
);


ALTER TABLE public.role_requests OWNER TO postgres;

--
-- Name: role_requests_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.role_requests_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.role_requests_id_seq OWNER TO postgres;

--
-- Name: role_requests_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.role_requests_id_seq OWNED BY public.role_requests.id;


--
-- Name: roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.roles (
    id integer NOT NULL,
    name character varying(50) NOT NULL
);


ALTER TABLE public.roles OWNER TO postgres;

--
-- Name: roles_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.roles_id_seq OWNER TO postgres;

--
-- Name: roles_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.roles_id_seq OWNED BY public.roles.id;


--
-- Name: unfreeze_requests; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.unfreeze_requests (
    id integer NOT NULL,
    user_id integer NOT NULL,
    book_id integer,
    reason text NOT NULL,
    status character varying(20) DEFAULT 'pending'::character varying NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT unfreeze_requests_status_check CHECK (((status)::text = ANY ((ARRAY['pending'::character varying, 'approved'::character varying, 'rejected'::character varying])::text[])))
);


ALTER TABLE public.unfreeze_requests OWNER TO postgres;

--
-- Name: unfreeze_requests_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.unfreeze_requests_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.unfreeze_requests_id_seq OWNER TO postgres;

--
-- Name: unfreeze_requests_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.unfreeze_requests_id_seq OWNED BY public.unfreeze_requests.id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    login character varying(50) NOT NULL,
    password character varying(255) NOT NULL,
    email character varying(150) NOT NULL,
    display_name character varying(100) NOT NULL,
    role_id integer NOT NULL,
    is_frozen boolean DEFAULT false NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
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
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- Name: books id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books ALTER COLUMN id SET DEFAULT nextval('public.books_id_seq'::regclass);


--
-- Name: complaints id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.complaints ALTER COLUMN id SET DEFAULT nextval('public.complaints_id_seq'::regclass);


--
-- Name: genres id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.genres ALTER COLUMN id SET DEFAULT nextval('public.genres_id_seq'::regclass);


--
-- Name: reading_lists id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reading_lists ALTER COLUMN id SET DEFAULT nextval('public.reading_lists_id_seq'::regclass);


--
-- Name: reading_statuses id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reading_statuses ALTER COLUMN id SET DEFAULT nextval('public.reading_statuses_id_seq'::regclass);


--
-- Name: reviews id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews ALTER COLUMN id SET DEFAULT nextval('public.reviews_id_seq'::regclass);


--
-- Name: role_requests id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_requests ALTER COLUMN id SET DEFAULT nextval('public.role_requests_id_seq'::regclass);


--
-- Name: roles id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles ALTER COLUMN id SET DEFAULT nextval('public.roles_id_seq'::regclass);


--
-- Name: unfreeze_requests id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.unfreeze_requests ALTER COLUMN id SET DEFAULT nextval('public.unfreeze_requests_id_seq'::regclass);


--
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- Data for Name: book_genre; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.book_genre VALUES (1, 4);
INSERT INTO public.book_genre VALUES (1, 8);
INSERT INTO public.book_genre VALUES (2, 9);
INSERT INTO public.book_genre VALUES (2, 6);
INSERT INTO public.book_genre VALUES (3, 7);
INSERT INTO public.book_genre VALUES (3, 9);
INSERT INTO public.book_genre VALUES (4, 8);
INSERT INTO public.book_genre VALUES (5, 4);
INSERT INTO public.book_genre VALUES (5, 11);
INSERT INTO public.book_genre VALUES (6, 3);
INSERT INTO public.book_genre VALUES (6, 6);
INSERT INTO public.book_genre VALUES (7, 7);
INSERT INTO public.book_genre VALUES (7, 11);
INSERT INTO public.book_genre VALUES (8, 2);
INSERT INTO public.book_genre VALUES (9, 11);
INSERT INTO public.book_genre VALUES (10, 11);
INSERT INTO public.book_genre VALUES (10, 6);
INSERT INTO public.book_genre VALUES (11, 7);
INSERT INTO public.book_genre VALUES (11, 4);
INSERT INTO public.book_genre VALUES (12, 10);
INSERT INTO public.book_genre VALUES (12, 6);
INSERT INTO public.book_genre VALUES (13, 4);
INSERT INTO public.book_genre VALUES (13, 9);
INSERT INTO public.book_genre VALUES (14, 8);
INSERT INTO public.book_genre VALUES (14, 7);
INSERT INTO public.book_genre VALUES (15, 6);
INSERT INTO public.book_genre VALUES (16, 8);
INSERT INTO public.book_genre VALUES (16, 7);
INSERT INTO public.book_genre VALUES (17, 2);
INSERT INTO public.book_genre VALUES (17, 5);
INSERT INTO public.book_genre VALUES (18, 8);
INSERT INTO public.book_genre VALUES (18, 6);
INSERT INTO public.book_genre VALUES (19, 7);
INSERT INTO public.book_genre VALUES (20, 6);
INSERT INTO public.book_genre VALUES (20, 11);
INSERT INTO public.book_genre VALUES (21, 11);
INSERT INTO public.book_genre VALUES (22, 9);
INSERT INTO public.book_genre VALUES (22, 6);
INSERT INTO public.book_genre VALUES (23, 2);
INSERT INTO public.book_genre VALUES (24, 2);
INSERT INTO public.book_genre VALUES (24, 7);
INSERT INTO public.book_genre VALUES (25, 3);
INSERT INTO public.book_genre VALUES (25, 4);


--
-- Data for Name: books; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.books VALUES (1, 'Тайна потерянного маяка', 'История о смотрителе старого маяка и загадочных огнях у скал.', NULL, NULL, 1, false, '2025-01-15 10:00:00');
INSERT INTO public.books VALUES (2, 'Огни забытого города', 'Путешествие журналиста в город, который исчез с карт после войны.', NULL, NULL, 1, false, '2025-01-22 11:30:00');
INSERT INTO public.books VALUES (3, 'Северный ветер', 'Сага о семье поморов на берегу Белого моря.', NULL, NULL, 1, false, '2025-02-01 09:15:00');
INSERT INTO public.books VALUES (4, 'Голос пустыни', 'Дневник геолога во время экспедиции по Каракумам.', NULL, NULL, 1, false, '2025-02-14 14:20:00');
INSERT INTO public.books VALUES (5, 'Гнездо вороны', 'Психологический детектив о пропавшем ребёнке в маленьком посёлке.', NULL, NULL, 1, true, '2025-03-05 18:45:00');
INSERT INTO public.books VALUES (6, 'Трамвай в полночь', 'Городская мистическая повесть о пассажире, который никогда не выходит.', NULL, NULL, 1, false, '2025-03-20 12:10:00');
INSERT INTO public.books VALUES (7, 'Пыль на старых письмах', 'Три поколения семьи через переписку, найденную на чердаке.', NULL, NULL, 2, false, '2025-01-08 16:00:00');
INSERT INTO public.books VALUES (8, 'Вечер у реки', 'Сборник коротких рассказов о людях провинциального городка.', NULL, NULL, 2, false, '2025-01-30 08:50:00');
INSERT INTO public.books VALUES (9, 'Седьмой этаж', 'Трое жильцов одной коммунальной квартиры рассказывают одну и ту же историю.', NULL, NULL, 2, false, '2025-02-18 17:25:00');
INSERT INTO public.books VALUES (10, 'Зеркало без отражения', 'Философская притча о художнике потерявшем себя.', NULL, NULL, 2, false, '2025-03-02 13:40:00');
INSERT INTO public.books VALUES (11, 'Тень в коридоре', 'Роман о школьном учителе и тайне старого здания гимназии.', NULL, NULL, 2, false, '2025-03-25 11:00:00');
INSERT INTO public.books VALUES (12, 'Кукольный дом', 'Сатирическая комедия о провинциальном театре.', NULL, NULL, 3, false, '2025-01-12 19:30:00');
INSERT INTO public.books VALUES (13, 'Часы остановились в три', 'Детектив с элементами мистики в дореволюционном Петербурге.', NULL, NULL, 3, false, '2025-02-05 10:15:00');
INSERT INTO public.books VALUES (14, 'Песня старого моряка', 'Приключения отставного капитана и его внука на Балтике.', NULL, NULL, 3, false, '2025-02-22 15:00:00');
INSERT INTO public.books VALUES (15, 'Письмо без адреса', 'Молодая почтальонка пытается найти получателя странного письма.', NULL, NULL, 3, false, '2025-03-10 12:30:00');
INSERT INTO public.books VALUES (16, 'Дорога в никуда', 'Дорожная история двух студентов автостопом до Владивостока.', NULL, NULL, 3, false, '2025-03-28 09:00:00');
INSERT INTO public.books VALUES (17, 'Утренний туман', 'Лирическая повесть о деревне ранней осенью.', NULL, NULL, 4, false, '2025-01-18 07:45:00');
INSERT INTO public.books VALUES (18, 'Сад за стеной', 'Мальчик находит заброшенный сад и тайну предыдущих хозяев.', NULL, NULL, 4, false, '2025-02-08 14:00:00');
INSERT INTO public.books VALUES (19, 'Поезд из Владивостока', 'Семь дней в купе и семь историй случайных попутчиков.', NULL, NULL, 4, false, '2025-02-25 18:20:00');
INSERT INTO public.books VALUES (20, 'Клавиши и струны', 'Молодая пианистка и её учитель — драма за кулисами консерватории.', NULL, NULL, 4, false, '2025-03-15 16:40:00');
INSERT INTO public.books VALUES (21, 'Молчание комнат', 'Три поколения одной квартиры — три истории об одиночестве.', NULL, NULL, 4, false, '2025-04-01 11:20:00');
INSERT INTO public.books VALUES (22, 'Ключ от чердака', 'Подростки находят дневник своей прабабушки времён эвакуации.', NULL, NULL, 5, false, '2025-01-25 13:10:00');
INSERT INTO public.books VALUES (23, 'Запах старых книг', 'Букинист рассказывает истории людей, продававших ему свои библиотеки.', NULL, NULL, 5, false, '2025-02-12 17:55:00');
INSERT INTO public.books VALUES (24, 'Двадцать первая ночь', 'Цикл из двадцати одной новеллы про одну улицу старого Петербурга.', NULL, NULL, 5, false, '2025-03-08 20:00:00');
INSERT INTO public.books VALUES (25, 'Пропавший этаж', 'В московской многоэтажке исчезает целый этаж — расследование жильцов.', NULL, NULL, 5, false, '2025-03-30 10:30:00');


--
-- Data for Name: complaints; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.complaints VALUES (1, 8, 5, NULL, 'Слишком жестокие сцены без предупреждения.', '2025-04-25 14:00:00');
INSERT INTO public.complaints VALUES (2, 10, 10, NULL, 'Подозрение на плагиат другого автора.', '2025-05-15 17:00:00');
INSERT INTO public.complaints VALUES (3, 6, NULL, 14, 'Оскорбительный тон по отношению к автору.', '2025-05-01 12:00:00');
INSERT INTO public.complaints VALUES (4, 7, NULL, 21, 'Спам, отзыв не по делу.', '2025-05-15 10:30:00');
INSERT INTO public.complaints VALUES (5, 9, 6, NULL, 'Проверка обложки на нарушение авторских прав.', '2025-05-10 09:00:00');


--
-- Data for Name: genres; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.genres VALUES (1, 'name', 'description');
INSERT INTO public.genres VALUES (2, 'Классика', 'Произведения признанные классическими в литературе');
INSERT INTO public.genres VALUES (3, 'Фантастика', 'Книги о вымышленных мирах и фантастических событиях');
INSERT INTO public.genres VALUES (4, 'Детектив', 'Расследования преступлений и поиск истины');
INSERT INTO public.genres VALUES (5, 'Поэзия', 'Стихотворные произведения разных эпох и стилей');
INSERT INTO public.genres VALUES (6, 'Драма', 'Произведения раскрывающие острые жизненные конфликты');
INSERT INTO public.genres VALUES (7, 'Роман', 'Развёрнутые повествования о судьбах героев');
INSERT INTO public.genres VALUES (8, 'Приключения', 'Динамичные истории с путешествиями и опасностями');
INSERT INTO public.genres VALUES (9, 'Историческая проза', 'Художественные произведения на исторической основе');
INSERT INTO public.genres VALUES (10, 'Сатира', 'Произведения высмеивающие пороки общества');
INSERT INTO public.genres VALUES (11, 'Психологический роман', 'Глубокий анализ внутреннего мира персонажей');


--
-- Data for Name: reading_lists; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.reading_lists VALUES (1, 6, 1, 4, '2025-04-01 10:00:00');
INSERT INTO public.reading_lists VALUES (2, 6, 2, 4, '2025-04-11 14:00:00');
INSERT INTO public.reading_lists VALUES (3, 6, 4, 3, '2025-04-19 12:00:00');
INSERT INTO public.reading_lists VALUES (4, 6, 12, 2, '2025-05-21 09:00:00');
INSERT INTO public.reading_lists VALUES (5, 6, 24, 4, '2025-07-08 18:00:00');
INSERT INTO public.reading_lists VALUES (6, 6, 9, 4, '2025-05-09 11:00:00');
INSERT INTO public.reading_lists VALUES (7, 6, 18, 2, '2025-06-14 16:00:00');
INSERT INTO public.reading_lists VALUES (8, 7, 1, 4, '2025-04-04 09:30:00');
INSERT INTO public.reading_lists VALUES (9, 7, 2, 4, '2025-04-14 15:00:00');
INSERT INTO public.reading_lists VALUES (10, 7, 16, 3, '2025-06-08 13:00:00');
INSERT INTO public.reading_lists VALUES (11, 7, 18, 4, '2025-06-16 10:00:00');
INSERT INTO public.reading_lists VALUES (12, 7, 12, 4, '2025-05-23 17:00:00');
INSERT INTO public.reading_lists VALUES (13, 7, 4, 1, '2025-04-21 11:30:00');
INSERT INTO public.reading_lists VALUES (14, 7, 6, 1, '2025-04-29 19:00:00');
INSERT INTO public.reading_lists VALUES (15, 8, 3, 4, '2025-04-15 12:00:00');
INSERT INTO public.reading_lists VALUES (16, 8, 5, 4, '2025-04-23 21:00:00');
INSERT INTO public.reading_lists VALUES (17, 8, 7, 4, '2025-05-01 13:00:00');
INSERT INTO public.reading_lists VALUES (18, 8, 9, 4, '2025-05-09 11:30:00');
INSERT INTO public.reading_lists VALUES (19, 8, 21, 4, '2025-06-26 20:00:00');
INSERT INTO public.reading_lists VALUES (20, 8, 11, 3, '2025-05-17 10:00:00');
INSERT INTO public.reading_lists VALUES (21, 8, 13, 4, '2025-05-25 20:30:00');
INSERT INTO public.reading_lists VALUES (22, 8, 23, 2, '2025-07-04 09:30:00');
INSERT INTO public.reading_lists VALUES (23, 10, 1, 4, '2025-04-09 19:00:00');
INSERT INTO public.reading_lists VALUES (24, 10, 5, 4, '2025-04-25 18:00:00');
INSERT INTO public.reading_lists VALUES (25, 10, 7, 4, '2025-05-03 10:00:00');
INSERT INTO public.reading_lists VALUES (26, 10, 21, 3, '2025-06-28 15:00:00');
INSERT INTO public.reading_lists VALUES (27, 10, 11, 4, '2025-05-19 18:00:00');
INSERT INTO public.reading_lists VALUES (28, 10, 19, 4, '2025-06-20 13:00:00');
INSERT INTO public.reading_lists VALUES (29, 10, 3, 4, '2025-04-17 16:00:00');
INSERT INTO public.reading_lists VALUES (30, 10, 24, 2, '2025-07-10 19:00:00');


--
-- Data for Name: reading_statuses; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.reading_statuses VALUES (1, 'Заброшено');
INSERT INTO public.reading_statuses VALUES (2, 'В планах');
INSERT INTO public.reading_statuses VALUES (3, 'Читаю');
INSERT INTO public.reading_statuses VALUES (4, 'Прочитано');


--
-- Data for Name: reviews; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.reviews VALUES (1, 6, 1, 'Захватывающий сюжет, читал не отрываясь.', 9, '2025-04-02 14:30:00');
INSERT INTO public.reviews VALUES (2, 7, 1, 'Атмосфера погружает с первой страницы.', 10, '2025-04-05 18:00:00');
INSERT INTO public.reviews VALUES (3, 8, 1, 'Концовка предсказуемая, но в целом достойно.', 7, '2025-04-08 11:45:00');
INSERT INTO public.reviews VALUES (4, 10, 1, 'Очень понравились описания природы.', 8, '2025-04-10 20:15:00');
INSERT INTO public.reviews VALUES (5, 6, 2, 'Сложная многослойная история.', 9, '2025-04-12 15:00:00');
INSERT INTO public.reviews VALUES (6, 7, 2, 'Тяжёлая тема но очень сильная вещь.', 10, '2025-04-15 09:30:00');
INSERT INTO public.reviews VALUES (7, 8, 3, 'Семейная сага в лучших традициях жанра.', 9, '2025-04-16 12:20:00');
INSERT INTO public.reviews VALUES (8, 10, 3, 'Нравится язык автора.', 8, '2025-04-18 17:40:00');
INSERT INTO public.reviews VALUES (9, 6, 4, 'Интересный формат — читается как настоящий дневник.', 8, '2025-04-20 10:00:00');
INSERT INTO public.reviews VALUES (10, 7, 4, 'Местами затянуто но идея оригинальная.', 6, '2025-04-22 14:15:00');
INSERT INTO public.reviews VALUES (11, 8, 5, 'Самая жуткая книга что я читала за год.', 9, '2025-04-24 22:00:00');
INSERT INTO public.reviews VALUES (12, 10, 5, 'Не для слабонервных. Сильно.', 10, '2025-04-26 19:30:00');
INSERT INTO public.reviews VALUES (13, 6, 6, 'Мистика на грани реальности — обожаю такое.', 9, '2025-04-28 21:00:00');
INSERT INTO public.reviews VALUES (14, 7, 6, 'Финал смазал всё впечатление к сожалению.', 5, '2025-04-30 16:45:00');
INSERT INTO public.reviews VALUES (15, 8, 7, 'Плакала весь вечер. Очень душевно.', 10, '2025-05-02 13:20:00');
INSERT INTO public.reviews VALUES (16, 10, 7, 'Эпистолярный жанр — большая редкость, спасибо автору.', 9, '2025-05-04 11:00:00');
INSERT INTO public.reviews VALUES (17, 6, 8, 'Каждый рассказ — маленькая жемчужина.', 10, '2025-05-06 15:30:00');
INSERT INTO public.reviews VALUES (18, 7, 8, 'Есть слабые рассказы, но в целом сборник хорош.', 7, '2025-05-08 18:00:00');
INSERT INTO public.reviews VALUES (19, 8, 9, 'Гениальная композиция — три точки зрения на одно событие.', 10, '2025-05-10 12:15:00');
INSERT INTO public.reviews VALUES (20, 10, 9, 'Нужно перечитать чтобы оценить полностью.', 8, '2025-05-12 20:30:00');
INSERT INTO public.reviews VALUES (21, 6, 10, 'Философская проза не моё, не зашло.', 4, '2025-05-14 14:00:00');
INSERT INTO public.reviews VALUES (22, 7, 10, 'Тяжело, но того стоит.', 9, '2025-05-16 17:20:00');
INSERT INTO public.reviews VALUES (23, 8, 11, 'Школьная мистика без штампов — приятно удивлена.', 8, '2025-05-18 10:45:00');
INSERT INTO public.reviews VALUES (24, 10, 11, 'Подростки получились очень живыми.', 9, '2025-05-20 19:00:00');
INSERT INTO public.reviews VALUES (25, 6, 12, 'Смеялась до слёз местами.', 9, '2025-05-22 13:30:00');
INSERT INTO public.reviews VALUES (26, 7, 12, 'Сатира тонкая, актёров в провинциях узнаёшь сразу.', 10, '2025-05-24 16:00:00');
INSERT INTO public.reviews VALUES (27, 8, 13, 'Дореволюционный Петербург выписан великолепно.', 10, '2025-05-26 21:30:00');
INSERT INTO public.reviews VALUES (28, 10, 13, 'Детектив с мистикой — мой жанр, рекомендую.', 9, '2025-05-28 18:15:00');
INSERT INTO public.reviews VALUES (29, 6, 14, 'Очень добрая и тёплая книга про связь поколений.', 9, '2025-05-30 11:00:00');
INSERT INTO public.reviews VALUES (30, 7, 14, 'Балтика в книге как живая.', 8, '2025-06-01 14:45:00');
INSERT INTO public.reviews VALUES (31, 8, 15, 'Напомнило старые советские фильмы — ностальгия.', 7, '2025-06-03 12:30:00');
INSERT INTO public.reviews VALUES (32, 10, 15, 'Сюжет затянут, идея интереснее реализации.', 6, '2025-06-05 17:00:00');
INSERT INTO public.reviews VALUES (33, 6, 16, 'Молодёжная роуд-стори, читается на одном дыхании.', 8, '2025-06-07 20:00:00');
INSERT INTO public.reviews VALUES (34, 7, 16, 'Сам ездил автостопом — всё узнаваемо!', 9, '2025-06-09 15:20:00');
INSERT INTO public.reviews VALUES (35, 8, 17, 'Лирика и проза в одном — необычно.', 8, '2025-06-11 09:30:00');
INSERT INTO public.reviews VALUES (36, 10, 17, 'Спокойная медитативная книга, хорошо для отпуска.', 7, '2025-06-13 13:00:00');
INSERT INTO public.reviews VALUES (37, 6, 18, 'Детская книга для взрослых — очень тёплая.', 9, '2025-06-15 16:45:00');
INSERT INTO public.reviews VALUES (38, 7, 18, 'Хочется посадить свой сад после прочтения.', 10, '2025-06-17 11:15:00');
INSERT INTO public.reviews VALUES (39, 8, 19, 'Семь историй — семь миров. Очень люблю такой формат.', 9, '2025-06-19 18:30:00');
INSERT INTO public.reviews VALUES (40, 10, 19, 'Купе как маленький театр — отличная метафора.', 9, '2025-06-21 14:00:00');
INSERT INTO public.reviews VALUES (41, 6, 20, 'Музыкальная драма со сложными отношениями.', 8, '2025-06-23 19:45:00');
INSERT INTO public.reviews VALUES (42, 7, 20, 'Учитель и ученица — банальный сюжет, хорошее исполнение.', 7, '2025-06-25 12:00:00');
INSERT INTO public.reviews VALUES (43, 8, 21, 'Очень глубоко об одиночестве. Зацепило.', 10, '2025-06-27 21:00:00');
INSERT INTO public.reviews VALUES (44, 10, 21, 'Не для всех, но кому надо — тот поймёт.', 8, '2025-06-29 16:30:00');
INSERT INTO public.reviews VALUES (45, 6, 22, 'Прабабушкин дневник — очень трогательно.', 10, '2025-07-01 13:15:00');
INSERT INTO public.reviews VALUES (46, 7, 22, 'Историческая часть отличная, современная слабее.', 7, '2025-07-03 17:45:00');
INSERT INTO public.reviews VALUES (47, 8, 23, 'Букинист — главный герой, и это работает!', 9, '2025-07-05 10:30:00');
INSERT INTO public.reviews VALUES (48, 10, 23, 'Каждая глава — отдельная новелла, очень хорошо.', 9, '2025-07-07 15:00:00');
INSERT INTO public.reviews VALUES (49, 6, 24, 'Двадцать одна история — двадцать одно настроение.', 10, '2025-07-09 20:15:00');
INSERT INTO public.reviews VALUES (50, 7, 24, 'Старый Петербург узнаётся, любителям города зайдёт.', 8, '2025-07-11 14:30:00');
INSERT INTO public.reviews VALUES (51, 8, 25, 'Городская мистика — мой любимый поджанр.', 9, '2025-07-13 18:00:00');
INSERT INTO public.reviews VALUES (52, 10, 25, 'Жильцы выписаны очень живо, узнаваемые типажи.', 8, '2025-07-15 11:45:00');
INSERT INTO public.reviews VALUES (53, 6, 9, 'Перечитала — стало ещё лучше.', 10, '2025-07-17 16:00:00');
INSERT INTO public.reviews VALUES (54, 7, 1, 'Ещё раз перечитал — и снова понравилось.', 9, '2025-07-19 19:30:00');
INSERT INTO public.reviews VALUES (55, 8, 2, 'Перечитываю на лето — всё актуально.', 9, '2025-07-21 12:15:00');


--
-- Data for Name: role_requests; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.role_requests VALUES (1, 6, 2, 'Готова делиться своими рассказами с сообществом.', 'pending', '2025-06-01 10:00:00');
INSERT INTO public.role_requests VALUES (2, 7, 2, 'Пишу более 5 лет, есть готовый сборник из 12 рассказов.', 'approved', '2025-05-20 14:30:00');
INSERT INTO public.role_requests VALUES (3, 8, 2, 'Хочу опубликовать первую повесть.', 'pending', '2025-07-10 16:00:00');
INSERT INTO public.role_requests VALUES (4, 10, 2, 'Пробовал в стол, теперь решил выйти на публику.', 'rejected', '2025-04-15 11:00:00');
INSERT INTO public.role_requests VALUES (5, 6, 2, 'Повторная заявка с готовой рукописью романа.', 'pending', '2025-07-15 09:30:00');


--
-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.roles VALUES (1, 'Читатель');
INSERT INTO public.roles VALUES (2, 'Автор');
INSERT INTO public.roles VALUES (3, 'Администратор');


--
-- Data for Name: unfreeze_requests; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.unfreeze_requests VALUES (1, 10, NULL, 'Аккаунт заморожен по ошибке после жалобы — прошу проверить.', 'pending', '2025-07-20 10:00:00');
INSERT INTO public.unfreeze_requests VALUES (2, 1, 5, 'Книга заморожена из-за жалобы — добавил предупреждение о контенте.', 'approved', '2025-05-10 12:00:00');
INSERT INTO public.unfreeze_requests VALUES (3, 1, 5, 'Повторная заявка после доработки книги.', 'pending', '2025-07-25 14:00:00');


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.users VALUES (1, 'tolstoy_a', '$2a$hash001', 'tolstoy@litportal.ru', 'Алексей Толстой', 2, false, '2026-05-04 18:22:20.424353');
INSERT INTO public.users VALUES (2, 'dostoyevsky_f', '$2a$hash002', 'dost@litportal.ru', 'Фёдор Достоевский', 2, false, '2026-05-04 18:22:20.424353');
INSERT INTO public.users VALUES (3, 'bulgakov_m', '$2a$hash003', 'bulgakov@litportal.ru', 'Михаил Булгаков', 2, false, '2026-05-04 18:22:20.424353');
INSERT INTO public.users VALUES (4, 'chekhov_a', '$2a$hash004', 'chekhov@litportal.ru', 'Антон Чехов', 2, false, '2026-05-04 18:22:20.424353');
INSERT INTO public.users VALUES (5, 'gogol_n', '$2a$hash005', 'gogol@litportal.ru', 'Николай Гоголь', 2, false, '2026-05-04 18:22:20.424353');
INSERT INTO public.users VALUES (6, 'anna_reader', '$2a$hash006', 'anna@mail.ru', 'Анна Смирнова', 1, false, '2026-05-04 18:22:20.424353');
INSERT INTO public.users VALUES (7, 'pavel_books', '$2a$hash007', 'pavel@mail.ru', 'Павел Иванов', 1, false, '2026-05-04 18:22:20.424353');
INSERT INTO public.users VALUES (8, 'marina_77', '$2a$hash008', 'marina@mail.ru', 'Марина Кузнецова', 1, false, '2026-05-04 18:22:20.424353');
INSERT INTO public.users VALUES (9, 'admin_root', '$2a$hash009', 'admin@litportal.ru', 'Главный администратор', 3, false, '2026-05-04 18:22:20.424353');
INSERT INTO public.users VALUES (10, 'oleg_reader', '$2a$hash010', 'oleg@mail.ru', 'Олег Сидоров', 1, true, '2026-05-04 18:22:20.424353');


--
-- Name: books_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.books_id_seq', 25, true);


--
-- Name: complaints_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.complaints_id_seq', 5, true);


--
-- Name: genres_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.genres_id_seq', 11, true);


--
-- Name: reading_lists_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.reading_lists_id_seq', 30, true);


--
-- Name: reading_statuses_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.reading_statuses_id_seq', 4, true);


--
-- Name: reviews_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.reviews_id_seq', 55, true);


--
-- Name: role_requests_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.role_requests_id_seq', 5, true);


--
-- Name: roles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.roles_id_seq', 3, true);


--
-- Name: unfreeze_requests_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.unfreeze_requests_id_seq', 3, true);


--
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 10, true);


--
-- Name: book_genre book_genre_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.book_genre
    ADD CONSTRAINT book_genre_pkey PRIMARY KEY (book_id, genre_id);


--
-- Name: books books_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books
    ADD CONSTRAINT books_pkey PRIMARY KEY (id);


--
-- Name: complaints complaints_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.complaints
    ADD CONSTRAINT complaints_pkey PRIMARY KEY (id);


--
-- Name: genres genres_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.genres
    ADD CONSTRAINT genres_name_key UNIQUE (name);


--
-- Name: genres genres_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.genres
    ADD CONSTRAINT genres_pkey PRIMARY KEY (id);


--
-- Name: reading_lists reading_lists_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reading_lists
    ADD CONSTRAINT reading_lists_pkey PRIMARY KEY (id);


--
-- Name: reading_lists reading_lists_user_id_book_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reading_lists
    ADD CONSTRAINT reading_lists_user_id_book_id_key UNIQUE (user_id, book_id);


--
-- Name: reading_statuses reading_statuses_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reading_statuses
    ADD CONSTRAINT reading_statuses_name_key UNIQUE (name);


--
-- Name: reading_statuses reading_statuses_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reading_statuses
    ADD CONSTRAINT reading_statuses_pkey PRIMARY KEY (id);


--
-- Name: reviews reviews_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT reviews_pkey PRIMARY KEY (id);


--
-- Name: role_requests role_requests_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_requests
    ADD CONSTRAINT role_requests_pkey PRIMARY KEY (id);


--
-- Name: roles roles_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_name_key UNIQUE (name);


--
-- Name: roles roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (id);


--
-- Name: unfreeze_requests unfreeze_requests_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.unfreeze_requests
    ADD CONSTRAINT unfreeze_requests_pkey PRIMARY KEY (id);


--
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- Name: users users_login_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_login_key UNIQUE (login);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- Name: book_genre book_genre_book_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.book_genre
    ADD CONSTRAINT book_genre_book_id_fkey FOREIGN KEY (book_id) REFERENCES public.books(id) ON DELETE CASCADE;


--
-- Name: book_genre book_genre_genre_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.book_genre
    ADD CONSTRAINT book_genre_genre_id_fkey FOREIGN KEY (genre_id) REFERENCES public.genres(id) ON DELETE CASCADE;


--
-- Name: books books_author_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books
    ADD CONSTRAINT books_author_id_fkey FOREIGN KEY (author_id) REFERENCES public.users(id);


--
-- Name: complaints complaints_book_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.complaints
    ADD CONSTRAINT complaints_book_id_fkey FOREIGN KEY (book_id) REFERENCES public.books(id);


--
-- Name: complaints complaints_review_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.complaints
    ADD CONSTRAINT complaints_review_id_fkey FOREIGN KEY (review_id) REFERENCES public.reviews(id);


--
-- Name: complaints complaints_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.complaints
    ADD CONSTRAINT complaints_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: reading_lists reading_lists_book_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reading_lists
    ADD CONSTRAINT reading_lists_book_id_fkey FOREIGN KEY (book_id) REFERENCES public.books(id);


--
-- Name: reading_lists reading_lists_status_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reading_lists
    ADD CONSTRAINT reading_lists_status_id_fkey FOREIGN KEY (status_id) REFERENCES public.reading_statuses(id);


--
-- Name: reading_lists reading_lists_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reading_lists
    ADD CONSTRAINT reading_lists_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: reviews reviews_book_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT reviews_book_id_fkey FOREIGN KEY (book_id) REFERENCES public.books(id);


--
-- Name: reviews reviews_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT reviews_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: role_requests role_requests_role_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_requests
    ADD CONSTRAINT role_requests_role_id_fkey FOREIGN KEY (role_id) REFERENCES public.roles(id);


--
-- Name: role_requests role_requests_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_requests
    ADD CONSTRAINT role_requests_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: unfreeze_requests unfreeze_requests_book_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.unfreeze_requests
    ADD CONSTRAINT unfreeze_requests_book_id_fkey FOREIGN KEY (book_id) REFERENCES public.books(id);


--
-- Name: unfreeze_requests unfreeze_requests_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.unfreeze_requests
    ADD CONSTRAINT unfreeze_requests_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: users users_role_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_role_id_fkey FOREIGN KEY (role_id) REFERENCES public.roles(id);


--
-- PostgreSQL database dump complete
--

\unrestrict x95kI9jH7gRv7QSMjiZ5PtuOydD7dTc6fmkgI0FxZ0OBno7UTRIvKifa5HLNtTL

