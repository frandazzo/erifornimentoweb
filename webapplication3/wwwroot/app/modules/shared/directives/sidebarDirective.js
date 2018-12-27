app.directive('sidebar', function(AuthenticationService) {


	function link(scope, element, attrs, controller, transcludeFn) { 
		element.on('click', function(){
			 if ($( window ).width() < 576){
			    $('.sidebar').toggleClass('inactive');
			  }
		});

	}



  return {
    restrict: 'A',
    link: link
  };
});