/**
 * formReForm JavaScript for Form Layout, version 0.2
 * (c) 2008 Joe Lippeatt (joey@lippeatt.com)
 * 
 * See the latest revisions at http://code.google.com/p/formreform/
 * 
 * formReForm is freely distributable and open.  Please contribute improvements
 * back to the project.  
 * 
 * see included README.txt for license information
 * 
 */

/* ---------------------------------------
 * formReForm(formDivId)
 * 
 * formReForm Main Constructor
   --------------------------------------- */
FormReForm = function(formDivId){

	/* Optional overridable properties 
	 * ------------------------------- */
    this.outerDivClass50PctName = "field50Pct";
    this.outerDivClass100PctName = "field100Pct";
	this.outerDivCheckbox = "fieldCheckbox";
	
	this.labelDivClassName = "fieldItemLabel";
	this.inputDivClassName = "fieldItemValue";
	this.checkboxClassName = "fieldCheckboxItem";
	
	/* Modify the location of your formReForm.css file as needed */
	this.cssFileLocation = "formReForm.css";
	
	/* Update/change the width of the main form window, if needed */
	this.formContainerWidth = null;
	//this.formContainerWidth = '230px';
	//this.formContainerWidth = '400px';
	//this.formContainerWidth = '560px';
	//this.formContainerWidth = '700px';
	
	this._isIe = false;
	
	// formDivId required, creates formContainerDivObj
    if ((formDivId=='') || (formDivId==null)) {
		alert("No form found.");
		return false;
	}
	var formContainerDivObj = document.getElementById(formDivId);
    
    // create label collection
    var formLabelObjCollection = formContainerDivObj.getElementsByTagName("label");
    
	// create array of label and input objects
	var formObjArr = new Array();
	
	/* ---------------------------------------
	 * this is the starting point for resculpting
	 * the form.  this functino sets up basic
	 * requirements and calls the formReFormElement
	 * method.
	 * --------------------------------------- */

    this.doReForm = function(){

        this._isIe = this.browserChk();
        
        // prevent some flicker
        formContainerDivObj.style.visibility = 'hidden';
		
		if (this.formContainerWidth!=null)
			formContainerDivObj.style.width = this.formContainerWidth;
        
        if (formLabelObjCollection.length > 0) {
        
            for (var i = 0; i < formLabelObjCollection.length; i++) {

                // get the current label for value
                var thisLabelForAttribute = this.getElementAttribute(formLabelObjCollection[i], 'for');

				// get the current label options form the rel value
				var thisLabelRelAttribute = this.getElementAttribute(formLabelObjCollection[i], 'rel');

				// get the element this item is "for"
                var thisEl = document.getElementById(thisLabelForAttribute);
                
				this.formReFormElement(formLabelObjCollection[i], thisEl, thisLabelRelAttribute);
				
			}
        }
        
        formContainerDivObj.style.visibility = 'visible';
        
    }
    
	/* ---------------------------------------
	 * this.loadformReFormCss()
	 * if the css is not included as a link or 
	 * style attribute in the html file itself, 
	 * you can load the style by calling this 
	 * function
	 * --------------------------------------- */
    this.loadformReFormCss = function() {
		var cssObj = document.createElement('link');
		cssObj.setAttribute('type', 'text/css');
		cssObj.setAttribute('rel', 'stylesheet');
		cssObj.setAttribute('href', this.cssFileLocation);
		
		if ((cssObj!="undefined") && (cssObj!=null)) 
		{
			var headObj = document.getElementsByTagName('head');
			headObj[0].appendChild(cssObj);
		}
    }
    
	/* crude but simple check for IE */
	this.browserChk = function() {
		
		var u = navigator.userAgent;
		i = u.indexOf('MSIE');
		if (i >= 0) 
			return true;
		else
			return false;
	}

	function elementInfo(label,element,options) {
		this.label = label;
		this.element = element;
		this.options = options;
	}
	
	// retrieve the value of an element's attribute
	this.getElementAttribute = function(el, attr) {
		var returnVal = null;

		// handle IE by checking attributes
		if (this._isIe) 
		{
			for( var x = 0; x < el.attributes.length; x++ ) {
				if( el.attributes[x].nodeName.toLowerCase() == attr.toLowerCase() ) {
					returnVal = el.attributes[x].nodeValue;
					return returnVal;
				}
			}
		} else {
			// handle other browsers correctly
			returnVal = el.getAttribute(attr);
		}
		return returnVal;
		
	}
	
	/* ---------------------------------------
	 * formReFormElement does all the heavy lifting.  
	 * This function take the label and form elements 
	 * and sculpts out the new GUI.
	 * --------------------------------------- */
	this.formReFormElement = function(labelEl, formInputEl, options) 
	{
		
		var elType = this.getElementAttribute(formInputEl,'type');

		// outer div element holds both the label and the input object
		var outerDiv = document.createElement('div');
		// add the outer div to the form container
		formContainerDivObj.insertBefore(outerDiv,labelEl);

		// label div element holds and styles the label element
		var labelDiv = document.createElement('div');
		
		// input element div holds and styles the input element
		var inputDiv = document.createElement('div');

		// conditional formatting based on type of input
		if (elType != 'checkbox') {
		
			if (options == '100pct') 
				outerDiv.className = this.outerDivClass100PctName;
			else 
				outerDiv.className = this.outerDivClass50PctName;
			
			// class items
			labelDiv.className = this.labelDivClassName;
			inputDiv.className = this.inputDivClassName;

			// add the new labelDiv and inputDiv as children of the outerDiv
			outerDiv.appendChild(labelDiv);
			outerDiv.appendChild(inputDiv);
			
		} else {
			
			// class items
			outerDiv.className = this.outerDivCheckbox;
			labelDiv.className = this.labelDivClassName;
			inputDiv.className = this.checkboxClassName;

			// add the new label and input div's, but in reverse order
			outerDiv.appendChild(inputDiv);
			outerDiv.appendChild(labelDiv);
		}

		// reassign the labelEl and formInputEl
		labelDiv.appendChild(labelEl);
		inputDiv.appendChild(formInputEl);
	}
}


