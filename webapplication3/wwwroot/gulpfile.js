let gulp = require('gulp');
let watch = require('gulp-watch');
let postcss = require('gulp-postcss');
let autoprefixer = require('autoprefixer');
let cssvars = require('postcss-simple-vars');
let nested = require('postcss-nested');
let postcssimport = require('postcss-import');
let browserSync = require('browser-sync');

var reload = browserSync.reload;


// gulp.task('default', function(){
// 	console.log('ueeee gulp..');
// });

// gulp.task('html', function(){
// 	console.log('qualcosa sull\'html...');
// });





gulp.task('watch', ['browser-sync'], function(){



	watch('./**/*.html', reload);


	watch('./app/assets/devcss/**/*.css', ()=>{
		gulp.start('css');
	});



  watch("./app/assets/js/**/*.js", reload);
});


gulp.task('css', ['cssInjectApp'], function(){
  
});

gulp.task('cssInjectApp', [ 'angular-v1-css-app'], function(){
  return gulp.src('./app/assets/css/appstyle.css')
  .pipe(browserSync.stream());
});


gulp.task('angular-v1-css-app', function(){
  return gulp.src('./app/assets/devcss/appstyle.css')
    .pipe(postcss([postcssimport, cssvars, nested, autoprefixer]))
    .pipe(gulp.dest('./app/assets/css'));
});


gulp.task('browser-sync',  function() {
  browserSync.init({
        server: {
            baseDir: "./"
        }
    });
});


