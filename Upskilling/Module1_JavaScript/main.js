console.log("Welcome to the Community Portal");

window.addEventListener('load', () => {
  alert('Page fully loaded and ready!');
  initializePortal();
});

class EventItem {
  constructor(name, date, seats, category, location, price = 0) {
    this.name = name;
    this.date = new Date(date);
    this.seats = seats;
    this.category = category;
    this.location = location;
    this.price = price;
  }

  checkAvailability() {
    return this.seats > 0 && this.date >= new Date();
  }
}

const events = [
  new EventItem('Community Music Night', '2026-07-20', 12, 'music', 'square'),
  new EventItem('Baking Workshop', '2026-07-25', 8, 'workshop', 'hall'),
  new EventItem('Park Yoga Session', '2026-08-02', 0, 'community', 'park'),
  new EventItem('Street Art Festival', '2026-08-15', 15, 'community', 'square'),
  new EventItem('Coding Meetup', '2026-09-04', 10, 'workshop', 'hall')
];

let registrationCount = 0;

const state = {
  category: 'all',
  location: 'all',
  search: ''
};

const eventsContainer = document.querySelector('#eventsContainer');
const noEventsMessage = document.querySelector('#noEventsMessage');
const categoryFilter = document.querySelector('#categoryFilter');
const locationFilter = document.querySelector('#locationFilter');
const searchInput = document.querySelector('#searchInput');
const eventSelect = document.querySelector('#eventSelect');
const registrationForm = document.querySelector('#registrationForm');
const formMessage = document.querySelector('#formMessage');
const loadEventsBtn = document.querySelector('#loadEventsBtn');
const loadingStatus = document.querySelector('#loadingStatus');

function initializePortal() {
  renderEvents(events);
  populateEventSelect(events);
  attachEventListeners();
}

function attachEventListeners() {
  categoryFilter.onchange = () => {
    state.category = categoryFilter.value;
    renderEvents(events);
  };

  locationFilter.onchange = () => {
    state.location = locationFilter.value;
    renderEvents(events);
  };

  searchInput.onkeydown = (event) => {
    if (event.key === 'Enter') {
      event.preventDefault();
      state.search = searchInput.value.trim().toLowerCase();
      renderEvents(events);
    }
  };

  loadEventsBtn.onclick = () => {
    fetchEventsFromMockApi();
  };

  registrationForm.onsubmit = (event) => {
    event.preventDefault();
    submitRegistration();
  };
}

function addEvent(eventItem) {
  events.push(eventItem);
  renderEvents(events);
  populateEventSelect(events);
}

function filterEventsByCategory(category, callback) {
  const filtered = events.filter((eventItem) => {
    return category === 'all' || eventItem.category === category;
  });
  if (typeof callback === 'function') {
    callback(filtered);
  }
}

function createCategoryRegistrationTracker(category) {
  let total = 0;
  return () => {
    total++;
    console.log(`${category} registrations: ${total}`);
    return total;
  };
}

const communityTracker = createCategoryRegistrationTracker('community');

function registerUser(eventIndex, userName, userEmail) {
  try {
    const eventItem = events[eventIndex];
    if (!eventItem) {
      throw new Error('Selected event not found');
    }
    if (!eventItem.checkAvailability()) {
      throw new Error('Event is full or already past');
    }
    eventItem.seats--;
    registrationCount++;
    if (eventItem.category === 'community') {
      communityTracker();
    }

    renderEvents(events);
    populateEventSelect(events);
    return `Registration successful for ${eventItem.name}`;
  } catch (error) {
    console.error(error);
    return `Registration failed: ${error.message}`;
  }
}

function registerForEventByIndex(index) {
  try {
    const eventItem = events[index];
    if (!eventItem) {
      throw new Error('Event not found');
    }
    if (!eventItem.checkAvailability()) {
      throw new Error('Cannot register for this event');
    }
    eventItem.seats--;
    registrationCount++;
    if (eventItem.category === 'community') {
      communityTracker();
    }
    renderEvents(events);
    populateEventSelect(events);
    formMessage.textContent = `Registered for ${eventItem.name}! Seats left: ${eventItem.seats}`;
    formMessage.className = 'success';
  } catch (error) {
    formMessage.textContent = error.message;
    formMessage.className = 'error';
  }
}

function renderEvents(eventList) {
  const filtered = eventList.filter((eventItem) => {
    const matchesCategory = state.category === 'all' || eventItem.category === state.category;
    const matchesLocation = state.location === 'all' || eventItem.location === state.location;
    const matchesSearch = eventItem.name.toLowerCase().includes(state.search.toLowerCase());
    return matchesCategory && matchesLocation && matchesSearch && eventItem.checkAvailability();
  });

  eventsContainer.innerHTML = '';
  if (filtered.length === 0) {
    noEventsMessage.classList.remove('hidden');
    return;
  }
  noEventsMessage.classList.add('hidden');

  filtered.forEach((eventItem, index) => {
    const card = document.createElement('div');
    card.className = 'event-card';

    const eventInfo = document.createElement('div');
    const title = document.createElement('h3');
    title.textContent = eventItem.name;
    const details = document.createElement('p');
    details.innerHTML = `<strong>Date:</strong> ${eventItem.date.toDateString()}<br><strong>Location:</strong> ${eventItem.location}<br><strong>Seats:</strong> ${eventItem.seats}`;
    eventInfo.append(title, details);

    const registerButton = document.createElement('button');
    registerButton.textContent = 'Register';
    registerButton.disabled = !eventItem.checkAvailability();
    registerButton.onclick = () => registerForEventByIndex(events.indexOf(eventItem));

    card.append(eventInfo, registerButton);
    eventsContainer.append(card);
  });
}

function populateEventSelect(eventList) {
  eventSelect.innerHTML = '';
  const availableEvents = eventList.filter((eventItem) => eventItem.checkAvailability());
  if (availableEvents.length === 0) {
    const option = document.createElement('option');
    option.textContent = 'No events available';
    option.value = '';
    eventSelect.append(option);
    return;
  }
  availableEvents.forEach((eventItem, idx) => {
    const option = document.createElement('option');
    option.value = idx;
    option.textContent = `${eventItem.name} - ${eventItem.date.toDateString()} (Seats: ${eventItem.seats})`;
    eventSelect.append(option);
  });
}

function submitRegistration() {
  const name = registrationForm.elements['userName'].value.trim();
  const email = registrationForm.elements['userEmail'].value.trim();
  const selectedIndex = registrationForm.elements['eventSelect'].value;

  if (!name || !email || selectedIndex === '') {
    formMessage.textContent = 'Please complete all registration fields.';
    formMessage.className = 'error';
    return;
  }

  const message = registerUser(selectedIndex, name, email);
  formMessage.textContent = message;
  formMessage.className = message.startsWith('Registration successful') ? 'success' : 'error';
}

function fetchEventsFromMockApi() {
  loadingStatus.classList.remove('hidden');
  setTimeout(() => {
    const mockData = [
      { name: 'Open Mic Evening', date: '2026-07-30', seats: 14, category: 'music', location: 'hall' },
      { name: 'Garden Planting Day', date: '2026-08-05', seats: 5, category: 'community', location: 'park' }
    ];
    const newEvents = mockData.map((item) => new EventItem(item.name, item.date, item.seats, item.category, item.location));
    newEvents.forEach((eventItem) => addEvent(eventItem));
    loadingStatus.classList.add('hidden');
  }, 1200);
}

function addNewEventExample() {
  const newEvent = new EventItem('Local Coding Workshop', '2026-09-20', 10, 'workshop', 'hall');
  addEvent(newEvent);
}

const eventDetails = events.map((eventItem) => `${eventItem.name} on ${eventItem.date.toDateString()}`);
console.log('Event details:', eventDetails);

const musicEvents = events.filter((eventItem) => eventItem.category === 'music');
console.log('Music events:', musicEvents.map((eventItem) => eventItem.name));

const clonedEvents = [...events];
console.log('Cloned events array:', clonedEvents);

const eventEntries = Object.entries(events[0]);
console.log('Event object entries:', eventEntries);
