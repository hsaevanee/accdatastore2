angular.module('rootApp', ['ui.router', 'root.controllers', 'root.services'])

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
        controller: 'indexCtrl'
    })

    $urlRouterProvider.otherwise('/index');
});
