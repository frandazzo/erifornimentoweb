app.factory('UnauthenticatedErrorService', ['$location', '$injector', '$q', function($location,$injector, $q) {  
    var redirectImplementer = {
        responseError: function(response) {
            // utente non autenticato


            if (response.status == 401){
                
                var svc = $injector.get('AuthenticationService')
                svc._clearClientData();
               
               $location.path('/login');
               
            }

             return $q.reject(response);
            
        }
    };
    return redirectImplementer;
}]);