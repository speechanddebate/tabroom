var overlays = {},
    admin = false;

function HangoutOverlay() {
  // gapi.hangout.av.setLocalParticipantVideoMirrored(false);
  
  var prevImgRsc = null;
  var timerLoaded = false;
  
  function refreshFromUrl(dataUrl) {
    /*
    if (!timerLoaded) {
      overlays['timer'] = gapi.hangout.av.effects.createImageResource('https://www.tabroom.com/timer/hangout-timer/res/timerhi.png?_t=' + Date.now());
      overlays['timer'].showOverlay({
        position: { x: -0.25, y: 0.2 },
        scale: {
          magnitude: 0.5,
          reference: gapi.hangout.av.effects.ScaleReference.WIDTH
        }
      });
      timerLoaded = true;
    }
    */

    overlays['numbers'] = gapi.hangout.av.effects.createImageResource(dataUrl); 
    overlays['numbers'].showOverlay();
    if (prevImgRsc) prevImgRsc.dispose();
    prevImgRsc = overlays['numbers'];
  };
  
  return {
    setUrl: refreshFromUrl
  };
};

include('https://www.tabroom.com/timer/hangout-timer/src/StopWatch.js?_t=' + Date.now(), function() {

  var timers     = ['1', 'a', 'b'],
      canvas     = document.getElementById('img'),
      overlay    = new HangoutOverlay(),
      hovertimer = new HoverTimer(canvas, 10, canvas.height - 10),
      stopwatch  = {};

  // Process input parameters
  var appdata = gadgets.views.getParams()['appData'],
      params = {};
  if (appdata) {
    var paramsstr = appdata.split(';');
    for (var i = 0; i < paramsstr.length; i++) {
      var data = paramsstr[i].split(':');
      params[data[0]] = data[1];
    }
  }
  if ((params['admin'] && gapi.hangout.data.getValue('admin') == undefined) || gapi.hangout.data.getValue('admin') == gapi.hangout.getLocalParticipant().person.id) {
    document.getElementById('bodytag').className = 'admin';
    gapi.hangout.data.setValue('admin', gapi.hangout.getLocalParticipant().person.id);
    admin = true;
  }
  else {
    document.getElementById('bodytag').className = 'not-admin';
  }

  for (var i = 0; i < timers.length; i++) {
    var id = timers[i];
    stopwatch[id] = new StopWatch(id);
  }

  // Duration must be in seconds
  var timer = function(duration, id) {
    if (duration) {
      stopwatch[id].reset(duration);
      hovertimer.clear();

      // Each second
      stopwatch[id].handler = function() {
        // Update timer here
        var minutes = parseInt((stopwatch[id].duration - parseInt(stopwatch[id].elapsed)) / 60, 10);
        var seconds = parseInt((stopwatch[id].duration - parseInt(stopwatch[id].elapsed)) % 60, 10);
        if (minutes < 10) minutes = '0' + minutes;
        if (seconds < 10) seconds = '0' + seconds;
        stopwatch[id].timestr = minutes + ':' + seconds;
        if (stopwatch[id].running) document.getElementById('timer-' + id).value = stopwatch[id].timestr;

        var dataurl = hovertimer.drawToDataUrl(stopwatch);
        overlay.setUrl(dataurl);

        if (stopwatch[id].timestr == '00:00') {
          timer(null, id);
        }
      };
      
      stopwatch[id].start();
    }
    else {
      stop(id);
      overlay.setUrl(hovertimer.drawToDataUrl(stopwatch));
    }
  };

  var start = function(id) {
    var value = document.getElementById('timer-' + id).value;
    if (value != '00:00') {
      stopwatch[id].last = value;
      var minutes_and_seconds = stopwatch[id].last.split(':');
      var time = parseInt(minutes_and_seconds[0], 10) * 60 + parseInt(minutes_and_seconds[1], 10);
      timer(time, id);
      resume(id);
    }
  };

  var pause = function(id) {
    var button = document.getElementById('start-' + id);
    button.onclick = (function(opt) {
      return function() { resume(opt); };
    })(id);
    button.innerHTML = 'Resume';
    button.className = 'resume';
    stopwatch[id].pause();
  };

  var resume = function(id) {
    var button = document.getElementById('start-' + id);
    button.onclick = (function(opt) {
      return function() { pause(opt); };
    })(id);
    button.innerHTML = 'Pause';
    button.className = 'pause';
    var value = document.getElementById('timer-' + id).value,
        minutes_and_seconds = value.split(':'),
        time = parseInt(minutes_and_seconds[0], 10) * 60 + parseInt(minutes_and_seconds[1], 10);
    stopwatch[id].resume(time);
  };

  var stop = function(id) {
    var button = document.getElementById('start-' + id);
    button.onclick = (function(opt) {
      return function() { start(opt); };
    })(id);
    button.innerHTML = 'Start';
    button.className = 'start';
    stopwatch[id].stop();
  };

  var validate = function(id) {
    var start   = document.getElementById('start-' + id),
        message = document.getElementById('message'),
        value   = document.getElementById('timer-' + id).value;
    if (/^[0-9]{2}:[0-9]{2}$/.test(value)) {
      start.disabled = false;
      start.className = 'start';
      message.innerHTML = '';
      message.style.display = 'none';
      stopwatch[id].timestr = value;
    }
    else {
      start.disabled = true;
      start.className = 'disabled';
      message.innerHTML = 'Time format must be mm:ss';
      message.style.display = 'block';
      stopwatch[id].timestr = '00:00';
    }
    var dataurl = hovertimer.drawToDataUrl(stopwatch);
    overlay.setUrl(dataurl);
  };

  for (var i = 0; i < timers.length; i++) {
    var id = timers[i];
    document.getElementById('timer-' + id).onkeyup = (function(opt) {
      return function() { validate(opt); };
    })(id);

    document.getElementById('start-' + id).onclick = (function(opt) {
      return function() { start(opt); };
    })(id);
  }
  
  document.getElementById('timer-1').onkeydown = function() {
    if (document.getElementById('debate-time-selected')) document.getElementById('debate-time-selected').removeAttribute('id');
  };

  document.getElementById('reset').onclick = function() {
    if (stopwatch['1'].last) {
      document.getElementById('timer-1').value = stopwatch['1'].timestr = stopwatch['1'].last;
      stop('1');
      var dataurl = hovertimer.drawToDataUrl(stopwatch);
      overlay.setUrl(dataurl);
    }
  };

  var types = {
    highschool: [8, 5, 3],
    college: [9, 6, 3],
    british: [7]
  };

  var minutesToString = function(minutes) {
    if (minutes < 10) minutes = '0' + minutes;
    return minutes + ':00';
  };
  
  var select = function(type) {
    var container = document.getElementById('debate-time-options'),
        timer     = document.getElementById('timer-1');
    
    container.innerHTML = '';

    for (var i = 0; i < types[type].length; i++) {
      var value = types[type][i],
          a     = document.createElement('A');
      a.setAttribute('href', '#');
      a.setAttribute('data-value', value);
      a.setAttribute('class', 'debate-time-option');
      if (minutesToString(value) == timer.value) a.setAttribute('id', 'debate-time-selected');
      var t = document.createTextNode(value + ' min');
      a.appendChild(t);
      container.appendChild(a);
      a.onclick = function() {
        stop('1');
        timer.value = minutesToString(this.getAttribute('data-value'));
        if (document.getElementById('debate-time-selected')) document.getElementById('debate-time-selected').removeAttribute('id');
        this.setAttribute('id', 'debate-time-selected');
        validate('1');
      };
    }
  };

  document.getElementById('debate-type').onchange = function() {
    select(this.value);
  };
  
  select('highschool');

  document.getElementById('hide-prep').onclick = function() {
    var prep = document.getElementById('prep-timer');
    if (this.innerHTML == 'Hide') {
      prep.style.display = 'none';
      this.innerHTML = 'Show';
    }
    else if (this.innerHTML == 'Show') {
      prep.style.display = 'block';
      this.innerHTML = 'Hide';
    }
  };

  var stateUpdated = function(evt) {
    for (var key in stopwatch) {
      stopwatch[key].timestr = evt.state[key];
      document.getElementById('timer-' + key).value = evt.state[key];
    }
    var dataurl = hovertimer.drawToDataUrl(stopwatch);
    overlay.setUrl(dataurl);
  };

  if (admin) {
    var dataurl = hovertimer.drawToDataUrl(stopwatch);
    overlay.setUrl(dataurl);
  }
  else {
    var fields = ['1', 'a', 'b'];
    for (var i=0; i < fields.length; i++) {
      document.getElementById('timer-' + fields[i]).setAttribute('readonly', 'readonly');
    }
    gapi.hangout.data.onStateChanged.add(stateUpdated);
  }
});
