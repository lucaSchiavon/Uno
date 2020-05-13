"use strict";

// Load plugins
const autoprefixer = require("gulp-autoprefixer");
const browsersync = require("browser-sync").create();
const cleanCSS = require("gulp-clean-css");
const del = require("del");
const gulp = require("gulp");
const header = require("gulp-header");
const merge = require("merge-stream");
const plumber = require("gulp-plumber");
const rename = require("gulp-rename");
const sass = require("gulp-sass");
const uglify = require("gulp-uglify");

// Load package.json for banner
const pkg = require('./package.json');

// Set the banner content
const banner = ['/*!\n',
  ' * Start Bootstrap - <%= pkg.title %> v<%= pkg.version %> (<%= pkg.homepage %>)\n',
  ' * Copyright 2013-' + (new Date()).getFullYear(), ' <%= pkg.author %>\n',
  ' * Licensed under <%= pkg.license %> (https://github.com/BlackrockDigital/<%= pkg.name %>/blob/master/LICENSE)\n',
  ' */\n',
  '\n'
].join('');

// BrowserSync
function browserSync(done) {
  browsersync.init({
    server: {
      baseDir: "./"
    },
    port: 3000
  });
  done();
}

// BrowserSync reload
function browserSyncReload(done) {
  browsersync.reload();
  done();
}

// Clean vendor
function clean() {
    return del(["./wwwroot2/vendor/"]);
}

// Bring third party dependencies from node_modules into vendor directory
function modules() {
  // Bootstrap JS
  var bootstrapJS = gulp.src('./node_modules/bootstrap/dist/js/*')
    .pipe(gulp.dest('./wwwroot/vendor/bootstrap/js'));
  // Bootstrap SCSS
  var bootstrapSCSS = gulp.src('./node_modules/bootstrap/scss/**/*')
      .pipe(gulp.dest('./wwwroot/vendor/bootstrap/scss'));
  // ChartJS
  var chartJS = gulp.src('./node_modules/chart.js/dist/*.js')
      .pipe(gulp.dest('./wwwroot/vendor/chart.js'));
  // dataTables
  var dataTables = gulp.src([
      './node_modules/datatables.net/js/*.js',
      './node_modules/datatables.net-bs4/js/*.js',
      './node_modules/datatables.net-bs4/css/*.css'
    ])
      .pipe(gulp.dest('./wwwroot/vendor/datatables'));
  // Font Awesome
  var fontAwesome = gulp.src('./node_modules/@fortawesome/**/*')
      .pipe(gulp.dest('./wwwroot/vendor'));
  // jQuery Easing
  var jqueryEasing = gulp.src('./node_modules/jquery.easing/*.js')
      .pipe(gulp.dest('./wwwroot/vendor/jquery-easing'));
  // jQuery
  var jquery = gulp.src([
      './node_modules/jquery/dist/*',
      '!./node_modules/jquery/dist/core.js'
    ])
      .pipe(gulp.dest('./wwwroot/vendor/jquery'));
  return merge(bootstrapJS, bootstrapSCSS, chartJS, dataTables, fontAwesome, jquery, jqueryEasing);
}

// CSS task
function css() {
  return gulp
    .src("./scss/**/*.scss")
    .pipe(plumber())
    .pipe(sass({
      outputStyle: "expanded",
      includePaths: "./node_modules",
    }))
    .on("error", sass.logError)
    .pipe(autoprefixer({
      cascade: false
    }))
    .pipe(header(banner, {
      pkg: pkg
    }))
    .pipe(gulp.dest("./wwwroot/css"))
    .pipe(rename({
      suffix: ".min"
    }))
    .pipe(cleanCSS())
      .pipe(gulp.dest("./wwwroot/css"))
    .pipe(browsersync.stream());
}

// JS task
function js() {
  return gulp
    .src([
      './wwwroot/js/*.js',
      '!./wwwroot/js/*.min.js',
    ])
    .pipe(uglify())
    .pipe(header(banner, {
      pkg: pkg
    }))
    .pipe(rename({
      suffix: '.min'
    }))
      .pipe(gulp.dest('./wwwroot/js'))
    .pipe(browsersync.stream());
}

// Watch files
function watchFiles() {
  gulp.watch("./scss/**/*", css);
    gulp.watch(["./wwwroot/js/**/*", "!./wwwroot/js/**/*.min.js"], js);
  gulp.watch("./Views/**/*.cshtml", browserSyncReload);
}

// Define complex tasks
//Cancella la cartella vendor e quindi Copia i file che ci sono nei package node modules dentro la cartella di distribuzione
//degli artefatti (questo job is può anche eliminare)
const vendor = gulp.series(clean, modules);
//questo job è molto importante in quanto
//pulisce e ricopia dentro vendor i file artefatti prelevati da nodemodules (vendor)
//quindi minifica i file js e css (cscss a css)
//const build = gulp.series(vendor, gulp.parallel(css, js));
const build =  gulp.parallel(css, js);
//oltre a fare cio che fa build monitora i cambiamenti dei file scss e ad ogni cambiamento refrescia il browser
const watch = gulp.series(build, gulp.parallel(watchFiles, browserSync));

// Export tasks
//exports.css = css;
//exports.js = js;
//exports.clean = clean;
//exports.vendor = vendor;
exports.build = build;
//exports.watch = watch;
exports.default = build;
