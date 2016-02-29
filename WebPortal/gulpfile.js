var gulp = require("gulp");
var sass = require("gulp-sass");
var libPaths = {
    scripts: ["bower_components/lib/bootstrap-sass/**/*.js", "bower_components/lib/bootstrap-sass/**/*.scss", "bower_components/lib/bootstrap-sass/**/*.eot", "bower_components/lib/bootstrap-sass/**/*.svg", "bower_components/lib/bootstrap-sass/**/*.ttf", "bower_components/lib/bootstrap-sass/**/*.woff", "bower_components/lib/bootstrap-sass/**/*.woff2"]
};

gulp.task("default", function() {
});

gulp.task("copyLibs", function() {
    gulp.src(libPaths.scripts).pipe(gulp.dest("assets/lib/bootstrap"));
});

gulp.watch(["/assets/styles/scss/style.scss"], function() {
    gulp.src("/assets/styles/scss/style.scss")
        .pipe(sass({ includePaths: ["_/scss/"] }))
        .pipe(gulp.dest("/assets/styles/css/"));
    gulp.run("sass");
});