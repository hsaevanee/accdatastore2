angular.module('MapApp', ['ui.router', 'root.controllers', 'root.services', 'ngMap'])

.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider

    .state('root', {
        url: '/root',
        abstract: true,
        controller: 'rootCtrl'
    })

    .state('index', {
        url: '/index',
        templateUrl: 'templates/index.html',
        controller: 'MapCtrl'
    })

    $urlRouterProvider.otherwise('/index');
});
