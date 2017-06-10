var basePath = "../WebApplication1.Web/wwwroot/";

require.config({

    baseUrl: basePath + "app",

    paths: {
        'jquery': basePath + "lib/jquery/dist/jquery",
        'knockout': basePath + "lib/knockout/dist/knockout",
        'text': basePath + "lib/text/text",
        'bootstrap': basePath + "lib/bootstrap/dist/js/bootstrap",
        'components': basePath + "app/components",
        'services': basePath + "app/services"
    },

    shim: {
        "bootstrap": { "deps": ["jquery"] }
    }
});