export type UpdateCertificateModel = {
  id: number
  name?: string
  description?: string
  institutionName?: string
  location?: string
  isRemote?: boolean
  startDate?: string
  endDate?: string
  stillEngaged?: boolean
  credentialId?: string
  credentialUrl?: string
  type?: string
}
