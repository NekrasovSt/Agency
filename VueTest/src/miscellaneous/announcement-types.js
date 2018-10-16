export const announcementTypes = {
  'Buy': 'Куплю',
  'Sell': 'Продам',
  'Lease': 'Сдам',
  'Rent': 'Сниму'
};
export const count = Object.values(announcementTypes).length;

export function getNameFromType(type) {
  return announcementTypes[type];
}
