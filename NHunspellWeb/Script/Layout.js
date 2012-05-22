// Copyright 2008 MSE-iT - Thomas Maierhofer
// All rights reserved

  	$(document).ready(function() 
	{
	    $.browser.version = $.browser.msie && parseInt($.browser.version) == 6 && window["XMLHttpRequest"] ? "7.0" : $.browser.version;

        if( $.browser.msie  && parseInt(jQuery.browser.version) < 7 ) { // IE6 Fixes und Broser Update Info
            $("#LayoutContainer").css("margin-top","0em");
            
            $("#LayoutContainer").prepend('<div style="background-color:#FFFFE1; border: 1px #A0A0A0 solid; font-size:75%; padding: 0.2em; margin-bottom:0.3em; ">'
            + 'Website Layout wird nicht unterstützt. Zur korrekten Darstellung aktualiseren Sie ihren Browser auf Internet Explorer 7, Firefox, Opera oder Safari.</div>');
            
            $("#News").css("float","left"); // News Div floaten um Text Wrap zu verhindern
            $("#Navigation .navlinks > li").removeClass("active"); // Aktiver link entfernen weil der IE6 das nicht richtig layoutet
        }
	
 	    if( ! $.browser.msie  || parseInt(jQuery.browser.version) > 6 ) // Support begins with IE7
 	    {

            // ******* Canvas einbauen *********            
            $("#Navigation").backgroundCanvas();                    // Gesamter Ribbon
            $("#Navigation .navgroups > li").backgroundCanvas();    // Einzelne Gruppe    
            
            $("#ContentLayoutContainer").backgroundCanvas();
            
            $("#Footer").backgroundCanvas();
            
            DrawBackground();

            //Elemente transparent machen um im Canvas zeichnen zu kÖnnen.
            
 	        $("#Navigation .navgroups").css("background-color","transparent");
            $("#Navigation .navgroups").css("border-color","transparent");
 	        
 	        $("#Content .Article").css("background-color","transparent");
            $("#Content .Article").css("border-color","transparent");

 	        $("#Formular").css("background-color","transparent");
            $("#Formular").css("border-color","transparent");
            
 	        $("#Formular  fieldset").css("background-color","transparent");
            $("#Formular  fieldset").css("border-color","transparent");            
            
        }

 	    if( ! $.browser.msie ) // Spezielle nicht IE Formatierungen
 	    {        
           $("#Navigation .navlinks > li > a > img").css("margin-left","auto");
           $("#Navigation .navlinks > li > a > img").css("margin-right","auto");
        }
        
        
        
	});
	
	$(window).load(function () {LayoutContent(); DrawBackground(); });
    $(window).resize(function(){LayoutContent(); DrawBackground(); });
	
	
	
	// ****************** Kompletten Inhalt layouten ******************************
	// BODY <- LayoutContainer <- (Navigation, ContentLayoutContainer <- (Content, News), Footer)
	function LayoutContent()
	{
	    // ******************* News einblenden ******************************
	    var $LayoutContainer = $("#LayoutContainer");
	    var layoutContainerDimensions = $.boxDimensions( $LayoutContainer, {margins: true, padding:true, border:true } );
	    var width = $LayoutContainer.width();
	    
	    $Content = $("#Content");
	    $ContentDimensions = $.boxDimensions( $Content, {margins: true, padding:true, border:true } );
	    
	    if( width < 1200 )
	    {
	        $("#News").hide();
	        $Content.css("width","100%");
	    }
	    else
	    {
	        $Content.css("width",(width -300) + "px");
	        $("#News").show();
	    }
	    
   
	    
	    
	    // ************** Inhalt Positionieren **************************
        var contentWidth = $Content.width();
        var columnCount = Math.floor(contentWidth / 700) + 1;
        var articleWidth = contentWidth / columnCount;	    
            
        var columnIndex  = 0;
        var columnHeight = new Array();
        for( var index = 0; index < columnCount; ++ index )
        {
            columnHeight[index] = 0;
        }
        
        // Artikel     
	    $(".Article").each(
            function(){
                 $this = $(this);
                 
                 var boxDimensions =  $.boxDimensions( $this, {margins: true, padding:true, border:true } );
                 var innerWidth = articleWidth - boxDimensions.totalHorizontal;
                    // alert( articleWidth + " " + innerWidth );
                 $this.width( innerWidth );
                 $this.css("position","absolute");
                 
                 
                
                 var bestColumn = 0;
                 var lowestHeight = 9999999999;
                 for( var index = 0; index < columnCount; ++ index )
                 {
                    if( columnHeight[index] < lowestHeight )
                    {
                        lowestHeight = columnHeight[index];
                        bestColumn = index;
                        
                    }
                 }                 
                 
                 $this.css("top",columnHeight[bestColumn] + "px");
                 $this.css("left",bestColumn * articleWidth + "px");
                 columnHeight[bestColumn] += boxDimensions.totalVertical + $this.height();
                 
                 if((columnIndex % columnCount) == 0)
                 {
                    // $this.css("background-color","red");
                    //$this.css("float","left");
                    //$this.css("clear","left");
                 }
                 
                 
                 
                 ++columnIndex;
                 });
                 
        var maxHeight = 0;         
        
        // Artikel
        for( var index = 0; index < columnCount; ++ index )
        {
            if( columnHeight[index] > maxHeight )
                maxHeight = columnHeight[index];
            
        }  
        
        // Formular
	    $("#Formular").each(
            function(){
                 $this = $(this);
                 
                 var boxDimensions =  $.boxDimensions( $this, {margins: true, padding:true, border:true } );
                 maxHeight += boxDimensions.totalVertical + $this.height();                 
                 }); 
                 
       
       
        $Content.height(maxHeight);
        var contentLayoutContainerHeight = $("#News").height();
        contentLayoutContainerHeight = Math.max(contentLayoutContainerHeight,maxHeight); 
        $("#ContentLayoutContainer").height(contentLayoutContainerHeight);                
	    
    	// BODY <- LayoutContainer <- (Navigation, ContentLayoutContainer <- (Content, News), Footer)
	    
       	// Content Height  
	    var windowSize = GetBrowserWindowSize();
	    if ( windowSize.height != -1 )
	    {
	        var contentMinHeight = windowSize.height -10;
	        contentMinHeight -= $("#LayoutContainer").outerHeight();
	        contentMinHeight += $("#ContentLayoutContainer").innerHeight();
	        

	        $("#ContentLayoutContainer").css("min-height",contentMinHeight + "px");
            // window.alert( 'Height = ' + windowSize.height + " Content: " + contentMinHeight );
	    
	    }	    
	    
	}
	
	function GetBrowserWindowSize()
	{
	      var myWidth = -1, myHeight = -1;
          if( typeof( window.innerWidth ) == 'number' ) {
            // Standard Methode
            myWidth = window.innerWidth;
            myHeight = window.innerHeight;
          } else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) {
            //IE 6
            myWidth = document.documentElement.clientWidth;
            myHeight = document.documentElement.clientHeight;
          } else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) {
            // Anderer alter Scheiß
            myWidth = document.body.clientWidth;
            myHeight = document.body.clientHeight;
          }
          return {width:myWidth, height:myHeight};
	}
        

    
