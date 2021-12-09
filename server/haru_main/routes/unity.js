var express = require('express');
var fs = require('fs');
const path = require('path');
var http = require('http');
var request = require('request');

var router = express.Router();

/* GET home page. */
router.get('/', function(req, res, next) {
  res.sendFile(path.join('unity/index.html'));
});

router.get('/text', function(req, res, next){
  var text = req.query.req;
  var mon = new Date().getMonth() + 1;
  var day = new Date().getDate();
  var num = req.query.num;
  fs.writeFile( "public/texts/" + mon + day + "_" + num +".txt", '\ufeff' + text, 'utf8', function(error){
    console.log('write end')
  });
  //res.send("done");
});

router.get('/wather_request', function(req, res, next){
  console.log(req.query.req);
  var propertiesObject = { req: req.query.req }
  request({url:"http://studio522.iptime.org:21180/haru/weather.php", qs:propertiesObject}, function(err, response, body) {
    if(err) { console.log(err); return; }
    console.log("Get response: " + response.statusCode);
    res.send(response.body);
    res.end();
  });
});

router.get('/gpt_request', function(req, res, next){
  console.log(req.query.req);
  var propertiesObject = { req: req.query.req }
  request({url:"http://211.58.81.52:3000", qs:propertiesObject}, function(err, response, body) {
    if(err) { console.log(err); return; }
    console.log("Get response: " + response.statusCode);
    res.send(response.body);
    res.end();
  });
});

router.get('/translate', function(req, res, next){
  const options = {
    url: "https://openapi.naver.com/v1/papago/n2mt",
    headers: {
      "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8",
      "X-Naver-Client-Id": "sJPo04LGTMs2ap2YFvlE",
      "X-Naver-Client-Secret": "fDc0mgPU4S",
    },
    form:{
      source: "en",
      target: "ko",
      text: req.query.req
    }
  }

  request.post(options, function(err, response, body) {
    console.log("translate string is : " + req.query.req);
    if(err) { console.log(err); return; }
    console.log("Get response: " + response.statusCode);
    console.log("Get body: " + response.body);
    res.header('Content-type', 'text/html');
    res.send(response.body);
    res.end();
  });
});

/*
curl "https://openapi.naver.com/v1/papago/n2mt" \
-H "Content-Type: application/x-www-form-urlencoded; charset=UTF-8" \
-H "X-Naver-Client-Id: sJPo04LGTMs2ap2YFvlE" \
-H "X-Naver-Client-Secret: fDc0mgPU4S" \
-d "source=ko&target=en&text=만나서 반갑습니다." -v
*/


//http://211.58.81.52:3000?req=
module.exports = router;
