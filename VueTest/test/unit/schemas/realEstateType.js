export default {
  type: 'object',
  properties: {
    Id: {type: 'number'},
    Floor: {type: 'string'},
    Rooms: {type: 'number'},
    Square: {type: 'number'},
    Region: {type: 'string'},
    City: {type: 'string'},
    Street: {type: 'string'},
    Building: {type: 'string'},
    Code: {type: 'string'},
    RealEstateType: {
      'type': 'string',
      'enum': [
        'Apartment',
        'NewBuilding',
        'House'],
    },
  },
  required: [
    'Floor',
    'Rooms',
    'Square',
    'Region',
    'City',
    'Street',
    'Building',
    'Code',
    'RealEstateType'],
  additionalProperties: false
};
