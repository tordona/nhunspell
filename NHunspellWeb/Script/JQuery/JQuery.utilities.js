// Utilities


(function($) {
$.fn.reverseOrder = function() {
	return this.each(function() {
		$(this).prependTo( $(this).parent() );
	});
};
})(jQuery);

jQuery.fn.reverse = function() {
  return this.pushStack(this.get().reverse(), arguments);
};

jQuery.fn.sort = function() {
  return this.pushStack( [].sort.apply( this, arguments ), []);
};
