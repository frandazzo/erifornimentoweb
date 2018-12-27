app.controller('bodyController', function($scope, $location, AuthenticationService){


	$scope.$watch(

		function() { return AuthenticationService.authenticated; },
		function(){
			$scope.currentUser = AuthenticationService.currentUser;

			$scope.authenticated = AuthenticationService.authenticated;
	});

	
	

 	$scope.logout = function(){
			AuthenticationService.logout().then(function(){
				$location.path('login');
			});
		}
	
});