$(document).ready(function() {
  console.log($('#helloWorld').text());
  $('#headerTitle').text('jQuery Exercise Page');

  $('#hideButton').click(function() {
    $('.changeText').first().hide();
  });

  $('#hideBox').click(function() {
    $('#effectBox').hide();
  });

  $('#showBox').click(function() {
    $('#effectBox').show();
  });

  $('#fadeOutBox').click(function() {
    $('#effectBox').fadeOut();
  });

  $('#fadeInBox').click(function() {
    $('#effectBox').fadeIn();
  });

  $('#toggleBox').click(function() {
    $('#effectBox').toggle();
  });

  $('#chainBox').click(function() {
    $('#effectBox').slideUp().delay(1000).slideDown();
  });

  $('#addColorButton').click(function() {
    const color = $('#colorInput').val().trim();
    if (color === '') {
      return;
    }

    const listItem = $('<li></li>').text(color);
    $('#colorList').append(listItem);
    $('#colorBox').css('background-color', color);
  });

  $('#colorBox').dblclick(function() {
    $(this).css('background-color', 'white');
  });

  $('#eventBox').click(function() {
    $(this).css('background-color', '#d1fae5').text('Clicked!');
  });

  $('#eventBox').dblclick(function() {
    $(this).css('background-color', '#fef3c7').text('Double clicked!');
  });

  $('#eventBox').mouseenter(function() {
    $(this).css('border', '2px solid #2563eb');
  });

  $('#eventBox').mouseleave(function() {
    $(this).css('border', '1px solid #f59e0b');
  });

  $('#eventInput').keypress(function(event) {
    if (event.which === 13) {
      event.preventDefault();
      $('#eventBox').append('<br>Enter pressed: ' + $(this).val());
    }
  });
});
