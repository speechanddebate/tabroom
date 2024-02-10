//Silence the hundreds and hundreds of Jquery violations in console please
jQuery.event.special.touchstart = {
  setup: function( _, ns, handle ){
    this.addEventListener("touchstart", handle, { passive: true });
  }
};
jQuery.event.special.mousewheel = {
  setup: function( _, ns, handle ){
    this.addEventListener("mousewheel", handle, { passive: true });
  }
};
jQuery.event.special.touchmove = {
  setup: function( _, ns, handle ){
    this.addEventListener("touchmove", handle, { passive: true });
  }
};
