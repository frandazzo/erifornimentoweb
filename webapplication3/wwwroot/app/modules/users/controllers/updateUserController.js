app.controller('UpdateUserController', function($scope, $route, $rootScope, userServices, $routeParams, toastr, $location){

	$rootScope.activeTab =  $route.current.activeTab;
	
	userServices.getUserById($routeParams.id)
	.then(function(response){
		$scope.user=response.data;
	},
	function(error){
		toastr.error('Errore', error.statusText );
	});




	$scope.update= function(){

		
		userServices.updateUser($scope.user)
			.then(
				function(response){
					//toastr.success('', "Aggiornamento completato");
					$location.path('/user/' + $scope.user.id);
				},function(error){
					
					toastr.error('Errore', error.data.error );
				}
			);
	}
});