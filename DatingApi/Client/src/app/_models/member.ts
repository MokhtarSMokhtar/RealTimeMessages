import { Photo } from "./photo"

export interface Member {
    id: number
    userName: string
    photoUrl: any
    dateOfBirth: string
    knownAs: string
    created: string
    lastActive: string
    gender: string
    introduction: string
    lookingFor: string
    interests: string
    city: string
    age:number
    country: string
    photos: Photo[]
  }
  

  