var express = require('express');
var fs = require('fs');
var multer = require('multer');
//var upload = multer({dest: './uploads'});
const path = require('path');
const upload = multer({
  storage: multer.diskStorage({
    destination: function (req, file, cb) {
      cb(null, './public/uploads/');
    },
    filename: function (req, file, cb) {
      cb(null, new Date().valueOf() + path.extname(file.originalname));
    }
  }),
});

var router = express.Router();

/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('index', { title: 'Express' });
});


router.get('/texts', function(req, res, next){
  fs.readdir('public/texts', function(err, data){
    res.header('Content-type', 'text/html');
    res.send(data);
    res.end;
  })
});

router.get('/img', function(req, res, next){
  fs.readdir('public/uploads', function(err, data){
    var filename = data[Math.trunc(data.length * Math.random())];
    res.header('Content-type', 'text/html');
    res.send(filename);
  });
});

router.get('/imgs', function(req, res, next){
  fs.readdir('public/uploads', function(err, data){
    res.header('Content-type', 'text/html');
    res.send(data);
    res.end;
  })
});

router.post('/photos', upload.single('myFile'), function(req, res)
{
  console.log(req.body);
  console.log(req);

  res.render('index', { title: 'Express' });
})

module.exports = router;
