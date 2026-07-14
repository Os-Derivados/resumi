export class ApiPagination {
	public readonly skip: number
	public readonly take: number

	public constructor(skip: number = 0, take: number = 20) {
		this.skip = skip
		this.take = take
	}

	public static readonly isDefault = (pagination: ApiPagination) => pagination.skip === 0 && pagination.take === 20
}