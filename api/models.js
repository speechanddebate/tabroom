/**
 * File generated with tools/dbToModel generator (in the root of this project)
 * To generate again: `node ./tools/dbToModel`
 * @author Jonathan Diego Rodríguez Rdz. <jonathan@bquate.com>
 */
const {
  Model
} = require('objection');
const knex = require('knex');

let PanelModel;

/**
 * panel model
 * @extends Model
 */
PanelModel = class extends Model {

  /**
   * @override
   */
  static get tableName() {
    return 'panel';
  }

  /**
   * @override
   */
  static get jsonSchema() {
    return {
      type: 'object',
      required: [],
      search: ['letter','flight','flip','g_event','room_ext_id',],
      properties: {
        id: {
          type: 'integer'
        },
        letter: {
          type: 'string'
        },
        flight: {
          type: 'string'
        },
        bye: {
          type: 'integer'
        },
        started: {
          type: 'date-time'
        },
        bracket: {
          type: 'integer'
        },
        flip: {
          type: 'string'
        },
        flip_at: {
          type: 'date-time'
        },
        flip_status: {
          type: 'any'
        },
        publish: {
          type: 'integer'
        },
        room: {
          type: 'integer'
        },
        round: {
          type: 'integer'
        },
        g_event: {
          type: 'string'
        },
        room_ext_id: {
          type: 'string'
        },
        invites_sent: {
          type: 'integer'
        },
        timestamp: {
          type: 'integer'
        },
      }
    }
  }

  /**
   * @override
   */
  static get relationMappings() {
    return {
    };
  }

}

module.exports = {
  PanelModel: PanelModel.bindKnex(knex)
}
