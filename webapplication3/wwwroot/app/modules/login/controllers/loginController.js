app.controller('LoginController', function($scope, $location, AuthenticationService){

	$scope.error = '';

	$scope.login = function(){
		
		$scope.error = '';

		//dall'html recupero username e password (ngmodel)
		AuthenticationService.login($scope.username, $scope.password).then(
			function(response){
				$location.path('/');
			},
			function(error){
				$scope.error = error.data.error;
			});


	}



});