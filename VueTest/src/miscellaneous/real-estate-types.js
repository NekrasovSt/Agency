export const realEstateTypes = {
  'House': 'Дом',
  'NewBuilding': 'Новостройка',
  'Apartment': 'Квартира',
};
export const count = Object.values(realEstateTypes).length;

export function getNameFromType(type) {
    return realEstateTypes[type];
}
