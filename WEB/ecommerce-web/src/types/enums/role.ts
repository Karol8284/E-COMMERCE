export const Role = {
  User: 'User',
  Admin: 'Admin'
} as const;

export type Role = typeof Role[keyof typeof Role];