// Copyright 2008 MSE-iT - Thomas Maierhofer
// All rights reserved

   function RibbonBackgroundPaintFkt(context, width, height, canvas, $canvas, $canvasDiv, $content, $element )  
   {
            var borderWidth= 1;
            var radius = 14;
            
            // Ribbon Background
            var options = {height: height, width: width, radius: 14, radiusTL:23, radiusBL:0, radiusBR:0, border: 0 };

            context.fillStyle = "#99BBFF";  // Border Color
            $.canvasPaint.roundedRect(context,options);


    		var backgroundGradient = context.createLinearGradient(0, 0, 0,height); // Achtung Offset und volle Canvas Höhe setzen! 
            backgroundGradient .addColorStop(0, '#EEF5FA');
            backgroundGradient .addColorStop(1 ,'#D8E2F5');            
            // backgroundGradient .addColorStop(0, '#D8E2F5');
            // backgroundGradient .addColorStop(1 ,'#EEF5FA');     

            context.fillStyle =backgroundGradient ; //  "#F4F8FF"; // Background Color
            options.border = 1;
            $.canvasPaint.roundedRect(context,options);
   } 

  
   function RibbonGroupBackgroundPaintFkt(context, width, height, canvas, $canvas, $canvasDiv, $content, $element )  
   {
            var borderWidth= 1;
            var radius = 10;
            var options = {height: height, width: width, radius: radius };
            
            context.fillStyle = "#99BBFF";  // Border Color 
            $.canvasPaint.roundedRect(context,options);

            var $groupLabel = $content.children('span');
            var groupLabelHeight = $groupLabel.outerHeight();

            var groupGradient = context.createLinearGradient(0, 0, 0, height - borderWidth );
            // groupGradient.addColorStop(0, '#C7D4ED');
            // groupGradient.addColorStop(1 , '#D8E8F5');
            groupGradient.addColorStop(0, '#D8E2F5');
            groupGradient.addColorStop(1 ,'#EEF5FA'); 
            
            
            context.fillStyle = groupGradient;  // Links Color
            options = {height: height-groupLabelHeight, width: width, radius: radius, radiusBL:0, radiusBR:0, border: borderWidth, borderB: 0 };
            $.canvasPaint.roundedRect(context,options);

            context.fillStyle = "#C2D2F3";  // Group Label Color
            options = {y: height-groupLabelHeight, height: groupLabelHeight, width: width, radius: radius, radiusTL:0, radiusTR:0, border: borderWidth, borderT: 0 };
            $.canvasPaint.roundedRect(context,options);
   } 
   
   
      function ContentBackgroundPaintFkt(context, width, height, canvas, $canvas, $canvasDiv, $content, $element )  
   {
   
            var contentWidth = $("#Content").outerWidth();
            var newsWidth = width-contentWidth;
            var newsDistance = 5;
            var newsBoxDistance = 4;
            var newsBoxBorderWidth= 1;
            var newsBoxRadius= 0;
            var borderWidth= 1;
            var radius = 0;
            var options = {x:0, height: height, width: contentWidth, radius:0, radiusTL: 0, radiusTR: 0, border: 0 };
            
            
            var backgroundGradient = context.createLinearGradient(0, borderWidth, 0, height - (2*borderWidth));
            backgroundGradient.addColorStop(0, '#D8E2F5');
            backgroundGradient.addColorStop(1 ,'#EEF5FA');                            
            
            if( newsWidth > 0 )
            {
                context.fillStyle = "#FFFFFF"; // Border
                options.radiusTL = 0;
                options.x= contentWidth + newsDistance;
                options.width = newsWidth - newsDistance;
                $.canvasPaint.roundedRect(context,options);
                
                options.border = borderWidth;
                context.fillStyle = backgroundGradient;  
                $.canvasPaint.roundedRect(context,options);

                var newsBoxOptions = {x: options.x + newsBoxDistance, y:newsBoxDistance,  height: height - (2 *newsBoxDistance), width: options.width - (2* newsBoxDistance), 
                radiusTR:newsBoxRadius, radiusBR:newsBoxRadius, radiusTL:newsBoxRadius, radiusBL:newsBoxRadius,  border:0, borderT:0, borderB:0 };
                
 
                
                context.fillStyle = "#99BBFF";
                $.canvasPaint.roundedRect(context,newsBoxOptions);
                
                var $newsBoxHeader = $("#NewsBox span");
                var newsBoxHeaderHeight = $newsBoxHeader.outerHeight();
                
                
                newsBoxOptions.radiusTL = 0;
                newsBoxOptions.radiusTR = 0;
                newsBoxOptions.y += newsBoxHeaderHeight;
                newsBoxOptions.height -= newsBoxHeaderHeight;
                var newsBoxBackgroundGradient = context.createLinearGradient(0,  newsBoxOptions.y, 0,newsBoxOptions.height + newsBoxOptions.y);
                // newsBoxBackgroundGradient.addColorStop(0, '#D8E2F5');
                // newsBoxBackgroundGradient.addColorStop(1 ,'#EEF5FA');
                newsBoxBackgroundGradient.addColorStop(0, '#EEF5FA');
                newsBoxBackgroundGradient.addColorStop(1 ,'#EEF5FA');                 
                
                newsBoxOptions.border = newsBoxBorderWidth;
                newsBoxOptions.borderB= newsBoxBorderWidth;
                context.fillStyle = newsBoxBackgroundGradient;  
                $.canvasPaint.roundedRect(context,newsBoxOptions);      
                
                
                // newsBoxOptions.radiusTL = newsBoxRadius;
                // newsBoxOptions.radiusTR = newsBoxRadius;
                newsBoxOptions.radiusBL = 0;
                newsBoxOptions.radiusBR = 0;

                newsBoxOptions.borderB= 0;
                newsBoxOptions.borderT= newsBoxBorderWidth;
                                                                
                newsBoxOptions.y = newsBoxDistance;
                newsBoxOptions.height = newsBoxHeaderHeight;
                
                context.fillStyle = "#C2D2F3";  
                $.canvasPaint.roundedRect(context,newsBoxOptions);
                                        
                
                options.radiusTR = 0; // für nachfolgenden Content!
            }
            
            context.fillStyle = "#FFFFFF"; 
            options.radiusTL = radius;
            options.border = 0;
            options.x= 0;
            options.width = contentWidth;
            $.canvasPaint.roundedRect(context,options);
            
            options.border = borderWidth;
            context.fillStyle = backgroundGradient;  
            $.canvasPaint.roundedRect(context,options);
            

            
            // Artikel Layouten
            var articleCount = 0;
              $content.find(".Article").each(
            function(){ ++ articleCount; });
            
            $content.find(".Article").each(
            function(){
                 $this = $(this);
                 var offsetParent = $content.offset();
                 var offset = $this.offset({relativeTo:$content[0]});
                 var width = $this.outerWidth();
                 var height = $this.outerHeight();
                 
                 var blueBorderOptions = {x: offset.left - offsetParent.left , y: offset.top - offsetParent.top,  height: height, width: width, radius: 10, border:0};
                
                 
                context.fillStyle = "#DDD";
                $.canvasPaint.roundedRect(context,blueBorderOptions);
                blueBorderOptions.border = 1;
               
                
                blueBorderOptions.radius = blueBorderOptions.radius - blueBorderOptions.border;
                 context.fillStyle = "#FFFEFF"; // blueBorderBackgroundGradient; // "#FCFEFE"; // blueBorderBackgroundGradient; // "#FCFEFE"; //
                $.canvasPaint.roundedRect(context,blueBorderOptions);                 
            });
           
            // Formulare layouten
            $content.find("#Formular").each(
            function(){
                 $this = $(this);
                 var offsetParent = $content.offset();
                 var offset = $this.offset({relativeTo:$content[0]});
                 var width = $this.outerWidth();
                 var height = $this.outerHeight();
                 
                 var blueBorderOptions = {x: offset.left - offsetParent.left , y: offset.top - offsetParent.top,  height: height, width: width, radius: 10, border:0};
                
                 
                context.fillStyle = "#DDD";
                $.canvasPaint.roundedRect(context,blueBorderOptions);
                blueBorderOptions.border = 1;
               
               
                blueBorderOptions.radius = blueBorderOptions.radius - blueBorderOptions.border;
                 context.fillStyle = "#FFFEFF"; // blueBorderBackgroundGradient; // "#FCFEFE"; // blueBorderBackgroundGradient; // "#FCFEFE"; //
                $.canvasPaint.roundedRect(context,blueBorderOptions);                 
            });        
            
            
            $content.find("fieldset").each(
            function(){
                 $this = $(this);
                 var offsetParent = $content.offset();
                 var offset = $this.offset({relativeTo:$content[0]});
                 var width = $this.outerWidth();
                 var height = $this.outerHeight();
                 var fontSize = 12;
                 height -= fontSize;
                 offset.top += fontSize;
                 
                 
                 var blueBorderOptions = {x: offset.left - offsetParent.left , y: offset.top - offsetParent.top,  height: height, width: width, radius: 10, border:0};
                
                 
                context.fillStyle = "#99BBFF"; // "#DDD";
                $.canvasPaint.roundedRect(context,blueBorderOptions);
                blueBorderOptions.border = 1;
               
               
                blueBorderOptions.radius = blueBorderOptions.radius - blueBorderOptions.border;
                 context.fillStyle = "#FCFEFE"; // "#EEF5FA"; // blueBorderBackgroundGradient; // "#FCFEFE"; // blueBorderBackgroundGradient; // "#FCFEFE"; //
                $.canvasPaint.roundedRect(context,blueBorderOptions);                 
            });               
           

   } 
   
   
 
   
      function FooterBackgroundPaintFkt(context, width, height, canvas, $canvas, $canvasDiv, $content, $element )  
   {
            var borderWidth= 1;
            var radius = 14;
            var options = {x:0, height: height, width: width, radius:0, radiusBL: radius, radiusBR: radius, border: 0 };
            
            
            var backgroundGradient = context.createLinearGradient(0, 0, 0, height - (2*borderWidth));
            // backgroundGradient.addColorStop(0 ,'#D8E2F5');
            // backgroundGradient.addColorStop(1, '#C7D4ED');               

                backgroundGradient.addColorStop(0 ,'#EEF5FA');
                backgroundGradient.addColorStop(1, '#D8E2F5');
            
            context.fillStyle = "#FFFFFF"; 
            $.canvasPaint.roundedRect(context,options);
            
            options.border = borderWidth;
            context.fillStyle = backgroundGradient;  
            $.canvasPaint.roundedRect(context,options);


   }   
   
   
   	function DrawBackground(){
 	    if( ! $.browser.msie  || parseInt(jQuery.browser.version) > 6 ) // Support begins with IE7
 	    {
            $("#Navigation").backgroundCanvasPaint(RibbonBackgroundPaintFkt);
            // $("#Navigation .navgroups > li").backgroundCanvasPaint(RibbonGroupBackgroundPaintFkt);
            $("#ContentLayoutContainer").backgroundCanvasPaint(ContentBackgroundPaintFkt);
            $("#Footer").backgroundCanvasPaint(FooterBackgroundPaintFkt);
        }
	}
   
