app.directive('logoutToggler', function(AuthenticationService) {


	function link(scope, element, attrs, controller, transcludeFn) { 
		element.on('click', function(){
			$('#logout').toggleClass('show');
			// scope.logout();
		    return false; 
		});


		scope.$watch(
			function(){
				return AuthenticationService.authenticated;
			}, 
			function(value){

				if (value){
					element.text(AuthenticationService.currentUser.name);
				}
		});

		// scope.logout = function(){
		// 	console.log('ciaooooooooooooooo')
		// }

		
	}





  return {
    restrict: 'A',
    link: link,
    scope:{
    	
    }
  };



});