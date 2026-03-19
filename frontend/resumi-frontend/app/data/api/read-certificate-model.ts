export type ReadCertificateModel = {
  id: number
  resumeId: number
  name: string
  description: string
  institutionName: string
  location?: string
  isRemote: boolean
  startDate: string
  endDate?: string
  stillEngaged: boolean
  credentialId?: string
  credentialUrl?: string
  type: string
}
