//Scroll to top on button click
$(document).ready(function () {
    $("#return-to-top-button").click(function (event) {
        event.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 'slow');
        return false;
    });
});

//Mobile menu styling and logic
$(document).ready(function () {
    $(".password").hide();

    $(".menu-toggle-button").click(function () {
        $(".nav-link-and-decorative-rectangle-container").addClass("nav-link-and-decorative-rectangle-container-toggled");
        $(".blurred-background").addClass("blurred-background-toggled");
        $(".top-navbar-container").addClass("top-navbar-container-toggled");
        $(".nav-link-container").addClass("nav-link-container-toggled");
        $(".menu-exit-button").addClass("menu-exit-button-toggled");
        $(".nav-links").addClass("nav-links-toggled");
        $(".home-link-a").addClass("home-link-a-toggled");
        $(".menu-decorative-rectangle").addClass("menu-decorative-rectangle-toggled");
    });

    $(".menu-exit-button").click(function () {
        $(".blurred-background").removeClass("blurred-background-toggled");
        $(".top-navbar-container").removeClass("top-navbar-container-toggled");
        $(".nav-link-and-decorative-rectangle-container").removeClass("nav-link-and-decorative-rectangle-container-toggled");
        $(".nav-link-container").removeClass("nav-link-container-toggled");
        $(".menu-exit-button").removeClass("menu-exit-button-toggled");
        $(".nav-links").removeClass("nav-links-toggled");
        $(".home-link-a").removeClass("home-link-a-toggled");
        $(".menu-decorative-rectangle").removeClass("menu-decorative-rectangle-toggled");

    });

    document.addEventListener("scroll", function () {
        $(".nav-link-and-decorative-rectangle-container").removeClass("nav-link-and-decorative-rectangle-container-toggled");
        $(".blurred-background").removeClass("blurred-background-toggled");
        $(".top-navbar-container").removeClass("top-navbar-container-toggled");
        $(".nav-link-container").removeClass("nav-link-container-toggled");
        $(".menu-exit-button").removeClass("menu-exit-button-toggled");
        $(".nav-links").removeClass("nav-links-toggled");
        $(".home-link-a").removeClass("home-link-a-toggled");
        $(".menu-decorative-rectangle").removeClass("menu-decorative-rectangle-toggled");
    });

    $(document).mouseup(e => {
        if (!$("nav-link-container-toggled").is(e.target) && $(".nav-link-container-toggled").has(e.target).length === 0) {
            $(".nav-link-and-decorative-rectangle-container").removeClass("nav-link-and-decorative-rectangle-container-toggled");
            $(".blurred-background").removeClass("blurred-background-toggled");
            $(".top-navbar-container").removeClass("top-navbar-container-toggled");
            $(".nav-link-container").removeClass("nav-link-container-toggled");
            $(".menu-exit-button").removeClass("menu-exit-button-toggled");
            $(".nav-links").removeClass("nav-links-toggled");
            $(".home-link-a").removeClass("home-link-a-toggled");
            $(".menu-decorative-rectangle").removeClass("menu-decorative-rectangle-toggled");
        }
    });
});

//SwiperJS API initialization for displaying pictures in Databse Manipulation slide
const swiper = new Swiper('.swiper', {
    // Optional parameters
    loop: true,
    autoHeight: true,
    autoplay: {
        delay: 4000,
        pauseOnMouseEnter: true,
        disableOnInteraction: false,
    },
    speed: 800,
    zoom: {
        maxRatio: 3,
    },
    grabCursor: true,
    // If we need pagination
    pagination: {
        el: '.swiper-pagination',
    },

    // Navigation arrows
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});

$(".kickit-image").click(function () {
    $(".swiper").show();
});

//GSAP animations
$(document).ready(function () {
    gsap.registerPlugin(ScrollTrigger);

    var tlArrowButton = gsap.timeline();
    var tlLandingSection = gsap.timeline({ defaults: { duration: 1 } });
    var tlAvatarImg = gsap.timeline({
        scrollTrigger: {
            trigger: ".intro-paragraph",
            start: "center bottom"
        }
    });
    var tlSEOImg = gsap.timeline({
        scrollTrigger: {
            trigger: ".scroll-four-right-col",
            start: "center bottom"
        }
    });

    tlLandingSection.to(".slogan", { y: 0, opacity: 1, duration: .5, delay: .4 })
        .to(".ls-img-right", { x: 0, opacity: 1, duration: .4 })
        .to(".job-title-container", { y: 0, opacity: 1, duration: .4 })
        .to(".ls-img-left", { x: 0, opacity: 1, duration: .4 });

    tlArrowButton.to(".arrow-button-img", { y: 6, duration: .6 })
        .to(".arrow-button-img", { y: -3, duration: .6, repeat: -1, yoyo: true });

    tlAvatarImg.to(".avatar-img", { opacity: 1, duration: .01 })
        .to(".avatar-img", { "clip-path": "circle(100%)", duration: 2.2, ease: "circ" })
        .to(".avatar-img", { rotation: 1.5, duration: 1., ease: Linear.easeNone })
        .to(".avatar-img", { rotation: -1.5, duration: 1, ease: Linear.easeNone, repeat: -1, yoyo: true });

    tlSEOImg.from(".scroll-four-img", { scaleX: 1.07, duration: 2.1, repeat: -1, yoyo: true });
});