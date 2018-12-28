// Dom7
var $ = Dom7;

// Theme
var theme = 'auto';
if (document.location.search.indexOf('theme=') >= 0) {
    theme = document.location.search.split('theme=')[1].split('&')[0];
}

// Init App
var app = new Framework7({
    id: 'io.framework7.testapp',
    root: '#app',
    theme: 'md',
    data: function () {
        return {
            user: {
                firstName: 'John',
                lastName: 'Doe',
            },
        };
    },
    methods: {
        helloWorld: function () {
            app.dialog.alert('Hello World!');
        },
    },
    routes: routes,
    vi: {
        placementId: 'pltd4o7ibb9rc653x14',
    }
    
});

//$(document).on('page:init', '.page[data-name="shijuan"]', function (e,page) {
//    // Do something here when page with data-name="about" attribute loaded and initialized
//    var mySwiper = new Swiper('.swiper-container');
//    app.progressbar.set("#wanchengdu", 50);
//    $(this).on('change', 'input', function () {
//        mySwiper.slideNext();
//    });
//})