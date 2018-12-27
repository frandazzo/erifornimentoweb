var app = angular.module("app", ['ngRoute', 'ngAnimate', 'toastr', 'angular-loading-bar']);
//imposto la costante da utilizzare nei servizi per la connessione ai servizi rest
app.value('serverBaseUrl', 'http://localhost:4900/'); 
//la porta è 5000 o 3000 a seconda che faccio debug
//lato server con npm start oppure debug lato client con gulp watch
//app.value('serverBaseUrl', 'https://rocky-stream-78333.herokuapp.com/');



app.config(['$routeProvider', 'toastrConfig', '$httpProvider', function ($routeProvider, toastrConfig,$httpProvider) {

    $httpProvider.interceptors.push('UnauthenticatedErrorService');

    angular.extend(toastrConfig, {
        autoDismiss: false,
        maxOpened: 0,    
        newestOnTop: true,
        positionClass: 'toast-bottom-right',
        timeOut: 3000,
        extendedTimeOut: 1000,
        tapToDismiss: false,
        progressBar: true,
      });


    $routeProvider
        .when('/login', {
            controller: 'LoginController',
            templateUrl: '../../../../app/modules/login/views/login.html',
            activeTab: ''
           
        })
 
        .when('/', {
            controller: 'HomeController',
            templateUrl: '../../../../app/modules/home/views/home.html',
            activeTab: ''
            
        })

        .when('/configurazione', {
            controller: 'configurazioneController',
            templateUrl: '../../../../app/modules/configurazione/views/configurazioneView.html',
            activeTab: 'configurazione'
        })

        .when('/utenti', {
            controller: 'UtentiController',
            templateUrl: '../../../../app/modules/users/views/utenti.html',
            activeTab: 'utenti'
        })
        .when('/newuser', {
            controller: 'NewUserController',
            templateUrl: '../../../../app/modules/users/views/newUser.html',
            activeTab: 'utenti'
        })

        .when('/updateuser/:id', {
            controller: 'UpdateUserController',
            templateUrl: '../../../../app/modules/users/views/updateuser.html',
            activeTab: 'utenti'
        })

        .when('/user/:id', {
            controller: 'UserController',
            templateUrl: '../../../../app/modules/users/views/user.html',
            activeTab: 'utenti'
        })

 
        .otherwise({ redirectTo: '/login' });
}])
 
.run(['AuthenticationService', function (authServ, httpInitializer) {
    //inizializzo il codice per la sicurezza
    authServ.init();
   // 

}]);



