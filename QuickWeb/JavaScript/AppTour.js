

function HomeScreenTour() {

    var tour = new introJs(     

    );
    tour.setOptions({
        steps: [
            {
                element: '#lblStoreName',
                intro: 'Welcome to the tour of <b>Quick Dry Cleaning Software</b>. We would help you to explore the work flow of how to generate a new order using this software.',
                position: 'bottom'
            },
        {
            element: '#achrAddCustomer',
            intro: '<div style="width:200px">Click here, the system will take you to the "<b>Add Customer</b>" screen.</div>',
            position: 'right'
        }
    ]
      });


    tour.start();
    tour.setOption('doneLabel', 'Next step').oncomplete(function () {
        window.location.href = "../New_admin/frmNewCustomer.aspx?Add=true&IsTour=" + $('#hdnIsTour').val() + "";
        tour.exit();
    });

//      tour.onbeforechange(function (targetElement) {
//          /// alert("end of introduction");
//          if (targetElement.id === '') {
//              // alert(targetElement.id);            
//              window.location.href = "../New_admin/frmNewCustomer.aspx";
//        }
//         
//      });
      
}

function NewCustomerTour(StepIndex) {
 
    var tour1 = new introJs();
    tour1.setOptions({
        steps: [
            {
                element: '#pnlNewCustomer',
                intro: '<b>Welcome to Add Customer Screen:</b> Add customer details and click on Add Customer button. Name and Address fields are mandatory, rest all fields are optional.',
                position: 'right'
            },
        {
            element: '#achrNewBooking',
            intro: '<div style="width:200px"><b>Wow -</b> you have successfully added a customer. Now click this button and proceed to generate a new order for this customer.</div>',
            position: 'left'
        }
    ]
    });

    tour1.start();
    tour1.goToStep(StepIndex);

    if (StepIndex == "2") {        
        $(".introjs-skipbutton").text('Next step');
        $('#hdnChkLastControlTour').val('1');
    }
  
    tour1.setOption('doneLabel', 'Next step').oncomplete(function () {     
        window.open('../New_Booking/frm_New_Booking.aspx?CustBN=' + $('#hdnCustomercode').val() + '&IsTour=' + $('#hdnIsTour').val() + '', '_self');
        tour1.exit();
    });  
        
}
function BookingTour(StepIndex) {
    var tour2 = new Tour();
    tour2.addSteps([{
        element: "#txtQty",
        title: "Step 1",
        content: "This is order generation screen, prefilled with the details of the customer whom you just added. Add <b>Quantity</b> of one garment type"
    }, {
        element: "#txtName",
        title: "Step 2",
        content: 'Select <b>Garment</b> like "Shirt" or "Coat".'
    },
     {
         element: "#mytags",
         title: "Step 3",
         content: 'You can mention any special instruction or information around the garment. This may include <b>condition of the garment</b> like:<br/>"Faded",<br/>"Loose buttons",<br/>"Sensitive to chemicals"<br/>"Color Stain"<br/>"Color Bleed" etc<br/><br/>You may also mention <b>special instructions</b> like:<br/>"No Starch"<br/>"Hard Starch"<br/>"Folded"<br/>"Box Packed" etc.<br/><br/>Or you may mention <b>garment fabric or Brand</b> like:<br/>"Woolen"<br/>"Silk" <br/>"Nike"<br/>"A&F"<br/>"GAP" etc.'
     },
     {
         element: "#mytagsColor",
         title: "Step 4",
         content: 'You can mention any <b>color</b> of the garment like:<br/>"Red",<br/>"Blue"<br/>"Black" etc.'         
     },
     {
         element: "#txtProcess",
         title: "Step 5",
         content: 'Select the service that needs to be applied on this garment. For eg,<br/>Select <b>DC</b> for "Dry Cleaning"<br/>Select <b>LD</b> for "Laundry"<br/>Select <b>WC</b> for "Wet Cleaning"<br/>Select <b>SP</b> for "Steam Press"<br/>Select  <b>IO</b> for "Iron Only"<br/>Select <b>WF</b> for "Wash & Fold"<br/>Select <b>WI</b> for "Wash & Iron"<br/>'        
     },
     {
         element: "#txtRate",
         title: "Step 6",
         content: 'Price will automatically be loaded from the price list for the selected garment against the selected service. However, you can over ride the price my mentioning it here.',
         placement: "left"
     }
     ,
     {
         element: "#grdEntry_ctl01_imgBtnGridEntry",
         title: "Step 7",
         content: 'Press "<b>Add</b>" button to add this garment to the order and proceed to add more garments if you wish to.',
         placement: "left"
     }
     ,
     {
         element: "#btnSaveBooking",
         title: "Step 8",
         content: 'Set the <b>SMS</b> and <b>EMail</b> preference and Click "<b>Save</b>" button to submit this order.'
     }
    ]);
    // Initialize the tour    
    tour2.init();
    tour2.restart();
    tour2.goTo(StepIndex);
}