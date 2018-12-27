app.controller('NewUserController',  function($scope, $route, $rootScope, userServices, $location, toastr){

	$rootScope.activeTab =  $route.current.activeTab;
	$scope.user = {};
	$scope.user.role = "USER";

	$scope.createUser = function()  {
		userServices.createUser($scope.user)
			.then(
				function(response){
					$location.path('/user/'+ response.data.id);
					toastr.info('', "Creazione completa" );
				},function(error){
					toastr.error('Errore', error.data.error );
				}
			);
	}
	
});