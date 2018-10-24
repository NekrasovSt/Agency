import realEstateType from './realEstateType'

export default {
  properties: {
    Price: {type: 'number'},
    AnnouncementType: {
      'type': 'string',
      'enum': [
        'Sell',
        'Buy',
        'Rent',
        'Lease'],
    },
    RealEstateObject: realEstateType
  },
  additionalProperties: false
}
