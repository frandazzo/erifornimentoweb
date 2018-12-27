app.controller('HomeController', function($rootScope, $route, $http,serverBaseUrl, $scope, $location, AuthenticationService, toastr){

	$rootScope.activeTab =  $route.current.activeTab;
	 
	//  toastr.success('Hello world!', 'Toastr fun!');
	// toastr.info('We are open today from 10 to 22', 'Information');
	// toastr.error('Your credentials are gone', 'Error');
	// toastr.warning('Your computer is about to explode!', 'Warning');
	// toastr.success('Hello world!', 'Toastr fun!');
	

});