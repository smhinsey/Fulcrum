var gulp = require('gulp'),
  streamqueue = require('streamqueue'),
  minifyHtml = require('gulp-minify-html'),
  templateCache = require('gulp-angular-templatecache'),
  concat = require('gulp-concat'),
  rename = require('gulp-rename'),
  umd    = require('gulp-umd'),
  uglify = require('gulp-uglify');

gulp.task('bootstrap', function() {
  var stream = streamqueue({objectMode: true});
  stream.queue(
    gulp.src('./src/directives/decorators/bootstrap/*.html')
    .pipe(minifyHtml({
      empty: true,
      spare: true,
      quotes: true
    }))
    .pipe(templateCache({
      module: 'schemaForm',
      root: 'directives/decorators/bootstrap/'
    }))
    );
  stream.queue(gulp.src('./src/directives/decorators/bootstrap/*.js'));

  stream.done()
  .pipe(concat('bootstrap-decorator.js'))
  .pipe(umd({
    dependencies: function() {
      return [
        {name: 'schemaForm'},
      ];
    },
    exports: function() {return 'schemaForm';},
    namespace: function() {return 'bootstrapDecorator';}
    }))
  .pipe(gulp.dest('./dist/'))
  .pipe(uglify())
  .pipe(rename('bootstrap-decorator.min.js'))
  .pipe(gulp.dest('./dist/'));

});
