app.controller('UserController', function($scope, $route, $rootScope, userServices, $routeParams, toastr){

	$rootScope.activeTab =  $route.current.activeTab;


	
	
	userServices.getUserById($routeParams.id)
	.then(function(response){
		$scope.user=response.data;
	},
	function(error){
		toastr.error('Errore', error.statusText );
	});

	
});