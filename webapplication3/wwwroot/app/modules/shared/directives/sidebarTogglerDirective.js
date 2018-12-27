app.directive('sidebarToggler', function(AuthenticationService) {


	function link(scope, element, attrs, controller, transcludeFn) { 
		element.on('click', function(){
			$('.sidebar').toggleClass('inactive'); 
		});

	}



  return {
    restrict: 'A',
    link: link
  };
});