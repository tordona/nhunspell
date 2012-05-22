// Copyright 2008 MSE-iT - Thomas Maierhofer
// All rights reserved

(function($) {
    
    // Margin and Padding Information
	$.boxDimensions = function($element, options) {
	            options = $.extend({
                margins: false,
                padding: false,
                border: false,
                size: false
                }, options); 
	
	        var result = {};
	        
	        if( options.margins )
	        {
	            var ml = parseFloat( $element.css("margin-left") ); 
	            var mr = parseFloat( $element.css("margin-right") );
	            var mt = parseFloat( $element.css("margin-top") );
	            var mb = parseFloat( $element.css("margin-bottom") );
	            
                result = $.extend({
                    marginLeft: ml,
                    marginRight: mr,
                    marginTop: mt,
                    marginBottom: mb,
                    marginHorizontal: ml+mr,
                    marginVertical: mt+mb
                    }, result); 	            
	        }

	        if( options.padding )
	        {
	            var pl = parseFloat( $element.css("padding-left") ); 
	            var pr = parseFloat( $element.css("padding-right") );
	            var pt = parseFloat( $element.css("padding-top") );
	            var pb = parseFloat( $element.css("padding-bottom") );
	            
                result = $.extend({
                    paddingLeft: pl,
                    paddingRight: pr,
                    paddingTop: pt,
                    paddingBottom: pb,
                    paddingHorizontal: pl+pr,
                    paddingVertical: pt+pb
                    }, result); 	            
	        }

	        if( options.border )
	        {
	            var bl = parseFloat( $element.css("border-left-width") ); 
	            var br = parseFloat( $element.css("border-right-width") );
	            var bt = parseFloat( $element.css("border-top-width") );
	            var bb = parseFloat( $element.css("border-bottom-width") );
	            
                result = $.extend({
                    borderLeft: bl,
                    borderRight: br,
                    borderTop: bt,
                    borderBottom: bb,
                    borderHorizontal: bl+br,
                    borderVertical: bt+bb
                    }, result); 	            
	        }


	        if( options.size )
	        {
	                result = $.extend({
                    width: $element.width(),
                    height: $element.heighth()
                    }, result); 
	        }
	        
	        	      
	        if(options.margins && options.border && options.padding)
	        {
	                result = $.extend({
                        totalLeft: result.marginLeft + result.borderLeft + result.paddingLeft,
                        totalRight: result.marginRight + result.borderRight + result.paddingRight,
                        totalTop: result.marginTop + result.borderTop + result.paddingTop,
                        totalBottom: result.marginBottom + result.borderBottom + result.paddingBottom,
                        totalHorizontal: result.marginHorizontal + result.borderHorizontal + result.paddingHorizontal,
                        totalVertical: result.marginVertical + result.borderVertical + result.paddingVertical
                        }, result); 
                        
                    if( options.size )
                    {
	                    result = $.extend({
                            totalWidth: result.totalHorizontal + result.width,
                            totalHeight: result.totalVertical + result.height
                            }, result);

                    }
                    	            
	        }
	        

	        	        	        
 	        return result;
 	    };    

    // Shaping     
	$.fn.formFlow= function(shape, options) {
 	    $(this).each
 	    ( function() {

            options = jQuery.extend({
             step: 9,
             border: false,
             floatRight: false
            }, options); 	    
 	        
 	       
 	        var $this = $(this);
 	        var width = $this.width();
 	        var height = $this.height();

 	       $this.css("margin-bottom","-" + height + "px");
 	        
 	        var floatDirection = "left";
 	        
            if( options.floatRight )
            {
                floatDirection = "right";
 	            $this.css("margin-left","-" + width + "px");
 	        }
            else
 	            $this.css("margin-right","-" + width + "px");
 	        
 	        shape.sort();
 	        shape.reverse();
 	        
 	        // fix rounding differences
 	        var processedHeight = 0;
 	        var calculatedHeight = 0.5;
 	        for( var index = 1; index < shape.length; ++ index )
 	        {
 	  
 	            var startHeight = shape[index -1][0] * height;
 	            var startWidth = shape[index -1][1] * width;
 	            var endHeight = shape[index][0] * height;
 	            var endWidth = shape[index][1] * width;
 	            
 	            var steps = Math.ceil((startHeight - endHeight) / options.step);
 	             	            
 	            var heightStep = (startHeight - endHeight) / steps; // Caution: reverse processing! 
 	            var widthStep = (endWidth - startWidth) / steps; 
 	            
 	            for( var step = 0; step < steps; ++ step)
 	            {
 	                var divHeight = Math.floor( heightStep );
 	                var divWidth = Math.ceil( startWidth + (step * widthStep));
 	                processedHeight += divHeight;
 	                calculatedHeight += heightStep;
 	                
 	                // check if rounding difference bigger than 1px, and adjust this div 
 	                if(calculatedHeight - processedHeight >= 1 )
 	                {
 	                    ++ divHeight;
 	                    ++ processedHeight; 	                
 	                }
 	                
 	                var borderStyle = "";
 	                if( options.border )
 	                {
 	                    if( step == 0)
 	                        borderStyle = "border-top: 1px #FF0000 solid; margin-bottom: -1px;"
 	                    else
 	                        borderStyle = "border-top: 1px #00FF00 solid;margin-bottom: -1px;"
 	                    
 	                }
 	                
 	                
                    $this.after('<div style="font-size:1px; padding: 0px 0px 0px 0px; margin-top:0px; '+borderStyle+'float:' +floatDirection +'; clear:'+floatDirection+'; height:'+ divHeight + 'px; width:'+divWidth+'px;"></div>'); 	        
 	            }
 	            
 
 	        }
 	        
 	        
 	    });
 	    
        return this;
		};


   

})(jQuery)