angular.module('rootApp', ['ui.router', 'root.controllers', 'root.services', 'ngMap'])

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

         .state('list', {
             url: '/list/:seedcode/:centretype/:dataname/:sYear',
             templateUrl: 'templates/list.html',
             controller: 'listCtrl'
         })

    $urlRouterProvider.otherwise('/index');
});
