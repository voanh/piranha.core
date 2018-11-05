var gulp = require('gulp'),
    sass = require('gulp-sass'),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    rename = require("gulp-rename"),
    uglify = require("gulp-uglify");

var input = {
    js: [
        "Assets/js/html5sortable.js",
        "Assets/js/jquery.pjax.js",
        "Assets/js/piranha.blocks.js",
        "Assets/js/piranha.media.js",
        "Assets/js/piranha.tools.js",
        "Assets/js/startup.js"
    ]
};
var output = {
    js: "wwwroot/assets/js/script.js"
};

gulp.task('min:css', function () {
    return gulp.src('Assets/scss/style.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(cssmin())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest('wwwroot/assets/css'));
    });

gulp.task('min:editor', function () {
    return gulp.src('Assets/scss/editor.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(cssmin())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest('wwwroot/assets/css'));
    });

gulp.task("min:js", function () {
    return gulp.src(input.js, { base: "." })
        .pipe(concat(output.js))
        .pipe(gulp.dest("."))
        .pipe(uglify())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest("."));
});  