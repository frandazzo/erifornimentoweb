app.directive('navbarContentToggler', function(AuthenticationService) {


	function link(scope, element, attrs, controller, transcludeFn) { 
		element.on('click', function(){
			$('#navbarSupportedContent').toggleClass('show'); 
		});

	}



  return {
    restrict: 'A',
    link: link
  };
});